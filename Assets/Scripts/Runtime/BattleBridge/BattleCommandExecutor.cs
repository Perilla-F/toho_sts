using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattleCommandExecutor : MonoBehaviour
{

    #region フィールド
    [SerializeField] private BattleManager _battleManager;
    [SerializeField] private TimelineView _timelineView;

    private BattleSystem _battleSystem;
    private IBattleContext _context;
    private HeroUnit _hero;
    private IEnemyManager _enemy;
    private TimelineManager _timeline;
    private HandUIManager _hand;

    private Queue<ICardObj> _discardQueue = new();
    private bool _isDiscarding = false;

    #endregion

    #region 初期化処理

    public void Initialize(HeroUnit hero, IBattleContext battleContext, BattleSystem system, IEnemyManager enemy, TimelineManager timelineManager, HandUIManager handUIManager)
    {
        _hero = hero;
        _context = battleContext;
        _battleSystem = system;
        _enemy = enemy;
        _timeline = timelineManager;
        _hand = handUIManager;

        BattleEventBus.Card.OnDiscard += (card, ct) => RequestDiscardAsync(card, ct).Forget();
    }

    #endregion
    #region 命令

    public async UniTask HandleCardUsedFlow(ICardObj card, IBattleUnit target, CancellationToken ct)
    {
        _context.Discard.AddCard(card);
        _context.Hand.RemoveCard(card);
        // 1. ターゲットの確定 (CardDataのTargetTypeに基づいてリストを作成)
        List<IBattleUnit> finalTargets = DetermineTargets(card.Source.Data.CardEffectTarget, target);

        // 2. Contextの作成
        var context = new CardContext(_battleSystem, _hero, finalTargets);

        // 3. Actionの作成
        int executionTime = _timeline.CurrentTime + card.Delay;
        var cardAction = new PlayerActionEvent(_hero, card, context, executionTime);
        _timeline.AddEvent(cardAction);

        // 非同期で使用処理を開始
        await HandleCardUsedAsync(card, cardAction, ct);

        await _battleSystem.ProcessUntilTime(executionTime);
    }

    private async UniTask HandleCardUsedAsync(ICardObj card, PlayerActionEvent cardAction, CancellationToken ct)
    {
        await _hand.GetCardUI(card).MoveToCenterAsync(ct);
        await _timelineView.ConfirmAction(cardAction, ct);
        await _hand.DiscardCardAsync(card, ct);
    }

    private List<IBattleUnit> DetermineTargets(CardEffectTarget targetType, IBattleUnit selectedEnemy)
    {
        List<IBattleUnit> targets = new List<IBattleUnit>();
        switch (targetType)
        {
            case CardEffectTarget.Self:
                targets.Add(_hero);
                break;
            case CardEffectTarget.Enemy:
                targets.Add(selectedEnemy);
                break;
            case CardEffectTarget.AllEnemies:
                foreach (var enemy in _enemy.GetAllEnemies())
                {
                    targets.Add(enemy);
                }
                break;
            case CardEffectTarget.Random:
                targets.Add(_enemy.GetRandomAliveEnemy());
                break;
            case CardEffectTarget.Area:
                break;
            default:
                break;
        }

        return targets;
    }

    public async UniTask RequestDiscardAsync(ICardObj card, CancellationToken ct)
    {
        _discardQueue.Enqueue(card);
        if (_isDiscarding) return; // 既に実行中ならそのまま待機

        _isDiscarding = true;
        while (_discardQueue.Count > 0)
        {
            var target = _discardQueue.Dequeue();
            await _hand.DiscardCardAsync(target, ct);
            await UniTask.Delay(100, cancellationToken: ct); // 演出の間隔
        }
        _isDiscarding = false;
    }

    public async UniTask DiscardAllHandAsync(CancellationToken ct)
    {
        var cardsToDiscard = _context.Hand.Cards.ToList();

        // System側のデータ更新は先に済ませる
        foreach (var card in cardsToDiscard)
        {
            _context.Hand.RemoveCard(card);
            _context.Discard.AddCard(card);
        }

        // 演出をずらして開始するタスクリスト
        var animationTasks = new List<UniTask>();

        for (int i = 0; i < cardsToDiscard.Count; i++)
        {
            var card = cardsToDiscard[i];

            // 1. 少しずつ時間をずらして待機
            await UniTask.Delay(TimeSpan.FromMilliseconds(100), cancellationToken: ct);

            // 2. 演出を開始するが、awaitせずにタスクリストに追加
            // DiscardCardAsync 自体は演出完了まで待機するので、
            // WhenAll でまとめて待つことで「すべて捨て終わる」ことを保証する
            animationTasks.Add(_hand.DiscardCardAsync(card, ct));
        }

        // すべての演出が完了するのを待つ
        await UniTask.WhenAll(animationTasks);
    }

    #endregion
    #region 終了処理

    private void OnDestroy()
    {
        BattleEventBus.Card.OnDiscard -= (card, ct) => RequestDiscardAsync(card, ct).Forget();
    }

    #endregion
}