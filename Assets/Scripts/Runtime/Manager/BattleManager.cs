using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    #region フィールド

    [SerializeField] private Button _turnEndButton;

    private GameManager _game;
    private GameContext _gameContext;
    private BattleSystem _battleSystem;
    private IAudioManager _audio;
    private BattleContext _context;
    private HeroUnit _hero;
    private EnemyManager _enemy;
    private TimelineManager _timeline;
    private TimelineView _timelineView;
    private PlayerController _player;
    private HandUIManager _hand;
    private BattleViewRoot _view;

    private Dictionary<ICardObj, BattleCard> _cardMap = new Dictionary<ICardObj, BattleCard>();

    private UniTaskCompletionSource<bool> _discardTaskSource;

    #endregion

    #region 初期化処理

    private void Start()
    {
        _turnEndButton.onClick.AddListener(OnTurnEndButtonClicked);
    }

    public void Initialize(GameManager game, GameContext gameContext, HeroUnit hero, BattleContext battleContext, BattleSystem system, IAudioManager audio, EnemyManager enemy, PlayerController controller, TimelineManager timelineManager, TimelineView timelineView, HandUIManager hand, BattleViewRoot view)
    {
        _game = game;
        _gameContext = gameContext;
        _context = battleContext;
        _battleSystem = system;
        _audio = audio;
        _enemy = enemy;
        _timeline = timelineManager;
        _timelineView = timelineView;
        _player = controller;
        _hand = hand;
        _view = view;
        _hero = hero;

        _battleSystem.OnTurnStart += TurnStart;
        _battleSystem.OnCompleteTurnStartFlow += SelectPhaseStart;
        _battleSystem.OnTurnEnd += TurnEndFlow;
        _battleSystem.OnCardDrawn += HandleCardDrawn;
        _battleSystem.OnDiscard += HandleDiscard;
        _battleSystem.OnDiscardHand += HandleDiscardHandRequest;
        _battleSystem.OnConfirmCardEvent += ConfirmCardEvent;
        _context.Deck.OnChangedDeckCount += OnChangedDeckCount;
        _hand.OnCompleteDiscardAnimation += OnDiscardAnimationFinished;
        _context.Discard.OnChangedDiscardCount += OnChangedDiscardCount;
        _hero.Mana.OnChanged += OnManaChanged;

        _view.DeckView.UpdateDeckCount(battleContext.Deck.Count);
        _view.DiscardAreaView.UpdateDiscardCount(battleContext.Discard.Count);
        _view.ManaView.UpdateUI(_battleSystem.Hero.Mana.GetMana());

        BattleEventBus.OnCardHovered += PreviewTimelineIcon;
        BattleEventBus.OnCardActive += SetAllCardsBusyExcept;
    }

    #endregion

    #region Systemへの命令

    public async void StartBattle()
    {
        _battleSystem.StartBattle();
        await _view.BattleStart();
        //_audio.PlayBGM("battle", true);
    }

    private async void TurnEndFlow(CancellationToken ct)
    {
        await DiscardHandAsync(ct);
        _battleSystem.TurnEnd();
        _battleSystem.TurnStart(ct);
        TurnStart(_battleSystem.TurnCount, ct);
    }

    private async void SelectPhaseStart(CancellationToken ct)
    {
        await _battleSystem.PlayerSelectPhase(ct);
    }


    private async void HandleCardUsed(BattleCard card, BattleUnit target, CancellationToken ct)
    {
        if (!card.Card.Useable())
        {
            card.ResetPos();
            return;
        }

        card.ChangeState(new CardBusyState(card));

        // カードが使用されたら、購読を解除する（二重発火防止）
        card.OnCardUsed -= HandleCardUsed;
        _cardMap.Remove(card.Card);
        _context.Discard.AddCard(card.Card);

        _battleSystem.BattleContext.Hand.RemoveCard(card.Card);
        // 1. ターゲットの確定 (CardDataのTargetTypeに基づいてリストを作成)
        List<BattleUnit> finalTargets = DetermineTargets(card.Card.Source.Data.CardEffectTarget, target);

        // 2. Contextの作成
        var context = new CardContext(_battleSystem, _hero, finalTargets);

        // 3. Actionの作成
        int executionTime = _timeline.CurrentTime + card.Card.Delay;
        var cardAction = new PlayerActionEvent(_hero, card.Card, context, executionTime);
        _timeline.AddEvent(cardAction);

        // 非同期で使用処理を開始
        await HandleCardUsedAsync(card, cardAction, ct);
    }

    public async UniTask HandleCardUsedAsync(BattleCard card, PlayerActionEvent cardAction, CancellationToken ct)
    {
        await card.MoveToCenterAsync(ct);
        await _timelineView.ConfirmAction(cardAction, ct);
        await _hand.PlayDiscardAnimationAsync(card, ct);
    }

    private List<BattleUnit> DetermineTargets(CardEffectTarget targetType, BattleUnit selectedEnemy)
    {
        List<BattleUnit> targets = new List<BattleUnit>();
        switch (targetType)
        {
            case CardEffectTarget.Self:
                targets.Add(_hero);
                break;
            case CardEffectTarget.Enemy:
                targets.Add(selectedEnemy);
                break;
            case CardEffectTarget.AllEnemies:
                foreach (var enemy in _enemy.Enemies)
                {
                    targets.Add(enemy);
                }
                break;
            case CardEffectTarget.Random:
                foreach (var enemy in _enemy.Enemies)
                {
                    targets.Add(enemy);
                }
                break;
            case CardEffectTarget.Area:
                break;
            default:
                break;
        }

        return targets;
    }

    private void OnTurnEndButtonClicked()
    {
        // 1. ボタンを即座に無効化（連打防止）
        _turnEndButton.interactable = false;

        // 2. UIを閉じる演出などを開始
        _player.EndSelection();

        // 3. システムに合図を送る
        _player.NotifyTurnEnd();
    }

    #endregion

    #region UIへの命令

    private async void BattleStart() => await _view.BattleStart();
    private void OnChangedDeckCount(int count) => _view.DeckView.UpdateDeckCount(count);
    private void OnChangedDiscardCount(int count) => _view.DiscardAreaView.UpdateDiscardCount(count);
    private void OnManaChanged() => _view.ManaView.UpdateUI(_battleSystem.Hero.Mana.GetMana());

    private async void TurnStart(int turn, CancellationToken ct)
    {
        _turnEndButton.interactable = true;
        await _view.TurnStart(turn);
    }

    private async void HandleCardDrawn(ICardObj cardData, CancellationToken ct)
    {
        // 演出をキューに溜めて、順番に実行する仕組み（あるいは単純に待機）
        await PlayDrawSequence(cardData, ct);
    }

    private async UniTask PlayDrawSequence(ICardObj cardData, CancellationToken ct)
    {
        // 1. UIを生成してインスタンスを取得
        var cardUI = _hand.CreateCardUI(cardData);

        // 2. BattleManagerで購読
        cardUI.OnCardUsed += HandleCardUsed;

        // 3. データとUIの対応を記憶
        _cardMap[cardData] = cardUI;

        // 4. 演出ロジックを実行
        await _hand.PlayDrawAnimationAsync(cardUI, ct);
    }

    private async void HandleDiscard(ICardObj cardData, CancellationToken ct)
    {
        await PlayDiscardSequence(cardData, ct);
    }

    private async UniTask PlayDiscardSequence(ICardObj card, CancellationToken ct)
    {
        if (_cardMap.TryGetValue(card, out var cardUI))
        {
            _discardTaskSource = new UniTaskCompletionSource<bool>();
            cardUI.OnCardUsed -= HandleCardUsed;
            _cardMap.Remove(card);
            _context.Discard.AddCard(card);
            await _hand.PlayDiscardAnimationAsync(cardUI, ct);
            // 完了通知が来るまで待機
            await _discardTaskSource.Task;
        }
    }

    private void HandleDiscardHandRequest(CancellationToken ct)
    {
        // ここで非同期処理を呼ぶ
        _ = DiscardHandAsync(ct);
    }

    /// <summary>
    /// 手札を全て捨て札へ送る
    /// </summary>
    /// <returns></returns>
    public async UniTask DiscardHandAsync(CancellationToken ct)
    {
        // 各カードのアニメーション完了タスクをリストに貯める
        List<UniTask> animationTasks = new List<UniTask>();

        foreach (var card in _context.Hand.Cards.ToList())
        {
            // 1. 命令を出す（アニメーション終了を待つためのUniTaskをもらう）
            // 0.05fの遅延をここに持たせる
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
            _context.Hand.RemoveCard(card);

            // アニメーションを開始し、完了までを待つタスクを追加
            animationTasks.Add(PlayDiscardSequence(card, ct));
        }

        // 2. すべてのアニメーションが完了するのを待つ
        await UniTask.WhenAll(animationTasks);
    }

    public void OnDiscardAnimationFinished()
    {
        _discardTaskSource?.TrySetResult(true);
        _view.DiscardAreaView.UpdateDiscardCount(_context.Discard.Count);
    }

    public void SetAllCardsBusyExcept(ICardObj cardData)
    {
        var activeCard = _cardMap[cardData];
        foreach (var card in _view.HandView.GetCards())
        {
            if (card != activeCard)
            {
                card.ChangeState(new CardBusyState(card));
            }
        }
    }

    private void PreviewTimelineIcon(ICardObj card)
    {
        var previewEvent = new PreviewActionEvent(_hero, card, card.Source.Data.Delay);
        _timelineView.ShowPreview(previewEvent);
    }

    private void ConfirmCardEvent(PlayerActionEvent action, CancellationToken ct) { }

    #endregion

    #region 終了処理

    private void OnDestroy()
    {
        _battleSystem.OnTurnStart -= TurnStart;
        _battleSystem.OnCompleteTurnStartFlow -= SelectPhaseStart;
        _battleSystem.OnTurnEnd -= TurnEndFlow;
        _battleSystem.OnCardDrawn -= HandleCardDrawn;
        _battleSystem.OnDiscard -= HandleDiscard;
        _battleSystem.OnDiscardHand -= HandleDiscardHandRequest;
        _battleSystem.OnConfirmCardEvent -= ConfirmCardEvent;
        _context.Deck.OnChangedDeckCount -= OnChangedDeckCount;
        _hand.OnCompleteDiscardAnimation -= OnDiscardAnimationFinished;
        _context.Discard.OnChangedDiscardCount -= OnChangedDiscardCount;
        _hero.Mana.OnChanged -= OnManaChanged;
        _hand.OnCompleteDiscardAnimation -= OnDiscardAnimationFinished;
        BattleEventBus.OnCardHovered -= PreviewTimelineIcon;
        BattleEventBus.OnCardActive -= SetAllCardsBusyExcept;
    }

    #endregion
}
