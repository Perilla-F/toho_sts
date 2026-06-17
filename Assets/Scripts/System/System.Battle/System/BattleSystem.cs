using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour, IBattleSystem
{
    private EnemyManager _enemyManager;
    private PlayerController _player;
    private GameManager gameManager;
    public HeroUnit Hero { get; private set; }
    private TimelineManager _timelineManager;
    private BattlePhase phase = BattlePhase.TurnStart;
    public BattleContext BattleContext { get; private set; }
    private IAudioManager AudioService;


    public int TurnCount { get; private set; } = 1;

    private CancellationToken _ct;

    public event Action BattleStart;
    public event Action<int> OnTurnStart;
    public event Action<ICardObj, CancellationToken> OnCardDrawn;
    public event Action<ICardObj, CancellationToken> OnDiscard;
    public event Action<PlayerActionEvent, CancellationToken> OnConfirmCardEvent;

    public void Setup(BattleContext context, HeroUnit heroUnit, GameManager gameManager, EnemyManager enemy, PlayerController player, TimelineManager timelineManager, IAudioManager audioService)
    {
        BattleContext = context;
        Hero = heroUnit;
        _enemyManager = enemy;
        _player = player;
        _timelineManager = timelineManager;

        this.gameManager = gameManager;
        AudioService = audioService;

        //audioService?.PlayBGM("battle_theme", true);

        _ct = this.GetCancellationTokenOnDestroy();
    }

    /// <summary>
    /// バトル開始処理
    /// </summary>
    /// <returns></returns>
    public async UniTask StartBattle()
    {
        try
        {
            BattleStart?.Invoke();
            BattleContext.Deck.Shuffle();
            await BattleLoop(_ct);
        }
        catch (OperationCanceledException)
        {
            // キャンセルされた時の後処理
            Debug.Log("バトルが中断されました。");
        }

    }

    /// <summary>
    /// 1ターンの敵の行動をタイムラインに登録
    /// </summary>
    public void AddActionToTimeline()
    {
        foreach (var enemy in _enemyManager.Enemies)
        {
            var actionDatas = enemy.PlanTurn(BattleContext);
            foreach (var actionData in actionDatas)
            {
                int priority = enemy.EnemyType == EnemyType.Normal ? 2 : 1;
                var enemyEvent = new EnemyActionEvent(enemy, actionData, actionData.ScheduledTime, priority);
                _timelineManager.AddEvent(enemyEvent);
            }
        }
        _timelineManager.OnTimelineBuilt();
    }

    /// <summary>
    /// バトルプロセス
    /// </summary>
    /// <returns></returns>
    private async UniTask BattleLoop(CancellationToken ct)
    {
        UnityEngine.Debug.Log("Battle Start!");
        while (true)
        {
            await TurnStart(ct);
            await PlayerSelectPhase(ct);
            await TurnEnd();
        }
    }

    /// <summary>
    /// ターン開始処理
    /// </summary>
    /// <returns></returns>
    private async UniTask TurnStart(CancellationToken ct)
    {
        phase = BattlePhase.TurnStart;
        Debug.Log("=== Turn Start ===");

        OnTurnStart?.Invoke(TurnCount);
        Hero.Mana.RefleshMana();

        AddActionToTimeline();
        await Draw(Hero.DrawCount);
    }

    /// <summary>
    /// プレイヤー行動選択中
    /// </summary>
    /// <returns></returns>
    private async UniTask PlayerSelectPhase(CancellationToken ct)
    {
        phase = BattlePhase.PlayerSelect;
        Debug.Log("=== Player Select Phase ===");

        bool isTurnEnd = false;

        while (!isTurnEnd)
        {
            // 1. 選択開始を通知（UIを表示したり、カードを触れるようにする）
            _player.BeginSelection();

            // 2. 「行動確定」または「ターン終了」のどちらかが起きるまで待つ
            // UniTask.WhenAny を使うと、複数の「待ち」を統合できます
            var result = await UniTask.WhenAny(
                _player.WaitForActionChosenAsync(ct), // 行動選択完了を待つ
                _player.WaitForTurnEndAsync(ct)      // ターンエンド押下を待つ
            );

            if (result == 1) // ターン終了
            {
                isTurnEnd = true;
            }
            else
            {
                // 3. 行動が選択された場合の処理
                var card = _player.ChosenCard;
                int executionTime = _timelineManager.CurrentTime + card.Delay;
                var cardContext = _player.CardContext;
                var cardEvent = new PlayerActionEvent(Hero, card, cardContext, executionTime);

                _timelineManager.AddEvent(cardEvent);
                _timelineManager.OnTimelineBuilt();

                _player.ConfirmAction(); // 選択済みフラグなどをリセット
                OnConfirmCardEvent(cardEvent, ct);

                // 4. 時刻を進める（敵の行動などがここで走る）
                await ProcessUntilTime(executionTime, ct);
            }
        }

        // ターン終了ボタン押下後
        _player.EndSelection(); // UIを閉じる

        // タイムラインに残ったすべてのイベント（主に敵の行動など）を消化
        while (_timelineManager.HasEvents())
        {
            // 次のイベントの時刻まで進めて実行
            await _timelineManager.ExecuteNextEventAsync(BattleContext);
        }

        // ターン終了処理へ
        await TurnEnd();
    }

    /// <summary>
    /// タイムライン処理
    /// </summary>
    /// <param name="targetTime"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask ProcessUntilTime(int targetTime, CancellationToken ct)
    {
        // 目標時刻に達する前のイベントをすべて実行
        while (_timelineManager.HasEventsUntil(targetTime))
        {
            await _timelineManager.ExecuteNextEventAsync(BattleContext);
        }

        // イベントがない「空白の時間」を埋める（タイムラインの針を目標まで進める）
        _timelineManager.CurrentTime = targetTime;
    }

    /// <summary>
    /// ターン終了処理
    /// </summary>
    /// <returns></returns>
    private async UniTask TurnEnd()
    {
        phase = BattlePhase.TurnEnd;
        Debug.Log("=== Turn End ===");

        await DiscardHandAsync();
        TurnCount++;
    }

    public async UniTask Draw(int count)
    {
        await DrawMultipleAsync(count, _ct);
    }

    /// <summary>
    /// 最終ダメージ計算
    /// </summary>
    /// <param name="user"></param>
    /// <param name="target"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public int CalculateDamage(BattleUnit user, BattleUnit target, int value)
    {
        var damage = user.AttackBonus + value;
        if (target.Effects.Exists(e => e.Data.effectId == "broken")) damage += damage / 2;
        return damage;
    }

    private async UniTask DrawMultipleAsync(int count, CancellationToken ct)
    {
        for (int i = 0; i < count; i++)
        {
            var card = BattleContext.Deck.Draw();
            BattleContext.Hand.AddCard(card);

            OnCardDrawn?.Invoke(card, ct);

            await UniTask.Delay(TimeSpan.FromSeconds(0.15f), cancellationToken: _ct);
        }
    }

    public async UniTask DiscardAsync(ICardObj card, CancellationToken ct)
    {
        BattleContext.Hand.RemoveCard(card);

        OnDiscard?.Invoke(card, ct);
    }

    /// <summary>
    /// 手札を全て捨て札へ送る
    /// </summary>
    /// <returns></returns>
    public async UniTask DiscardHandAsync()
    {
        var cardsToDiscard = BattleContext.Hand.Cards.ToList();

        foreach (var card in cardsToDiscard)
        {
            await DiscardAsync(card, _ct);
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: _ct);
        }
    }

    /// <summary>
    /// バトル終了処理
    /// </summary>
    public void EndBattle(CancellationToken ct)
    {
        List<SourceCard> updatedDeck = BattleContext.Deck.GetDeckAsSourceCards();

        gameManager.UpdateDeckAfterBattle(updatedDeck);

        SceneManager.LoadScene("MapScene");
    }

}
