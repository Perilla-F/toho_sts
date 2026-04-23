using System;
using System.Collections.Generic;
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

    private ICardUIHandler _uiHandler;

    public int TurnCount { get; private set; } = 1;

    private CancellationToken _ct;

    public event Action BattleStart;
    public event Action<int> OnTurnStart;

    public void Setup(BattleContext context, HeroUnit heroUnit, GameManager gameManager, EnemyManager enemy, PlayerController player, TimelineManager timelineManager, ICardUIHandler uIHandler, IAudioManager audioService)
    {
        BattleContext = context;
        Hero = heroUnit;
        _enemyManager = enemy;
        _player = player;
        _uiHandler = uIHandler;
        _timelineManager = timelineManager;

        this.gameManager = gameManager;
        AudioService = audioService;

        //audioService?.PlayBGM("battle_theme", true);

        _ct = this.GetCancellationTokenOnDestroy();
    }

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
            await TurnEnd(ct);
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

        _player.BeginSelection();

        bool endTurn = false;

        while (!endTurn)
        {
            await UniTask.Yield(ct);

            // プレイヤーが行動を選択した場合
            if (_player.HasChosenAction)
            {
                var card = _player.ChosenCard;
                int actionTime = _timelineManager.CurrentTime + card.Delay;

                _timelineManager.AddEvent(new PlayerActionEvent(
                    Hero,
                    _player.ChosenCard,
                    _timelineManager.CurrentTime + _player.ChosenCard.Delay
                ));
                _timelineManager.OnTimelineBuilt();

                _player.ConfirmAction();

                // 行動発動時刻まで時間を進める（途中の敵行動などを処理）
                await ProcessUntilTime(actionTime, ct);

                // 行動発動後 → 再び選択可能に
                _player.BeginSelection();
            }

            // ターンエンドが押された場合
            if (_player.TurnEndRequested)
            {
                endTurn = true;
            }

        }

        // 残りイベントを全部処理
        await ProcessAllEvents(ct);
    }

    /// <summary>
    /// イベント処理
    /// </summary>
    /// <param name="targetTime"></param>
    /// <returns></returns>
    private async UniTask ProcessUntilTime(int targetTime, CancellationToken ct)
    {
        phase = BattlePhase.TimelineRunning;
        while (_timelineManager.HasEvents() && _timelineManager.PeekNextEvent().Time <= targetTime)
        {
            await _timelineManager.PopNextEvent(BattleContext);
        }
    }

    /// <summary>
    /// 残りイベント全処理
    /// </summary>
    /// <returns></returns>
    private async UniTask ProcessAllEvents(CancellationToken ct)
    {
        phase = BattlePhase.TimelineRunning;
        while (_timelineManager.HasEvents())
        {
            await _timelineManager.PopNextEvent(BattleContext);
        }
    }

    /// <summary>
    /// ターン終了処理
    /// </summary>
    /// <returns></returns>
    private async UniTask TurnEnd(CancellationToken ct)
    {
        phase = BattlePhase.TurnEnd;
        Debug.Log("=== Turn End ===");

        await DiscardAllAsync(ct);
        TurnCount++;
    }

    public async UniTask Draw(int count)
    {
        await DrawMultipleAsync(count, _ct);
    }

    private async UniTask DrawMultipleAsync(int count, CancellationToken ct)
    {
        for (int i = 0; i < count; i++)
        {
            var card = BattleContext.Deck.Draw();
            BattleContext.Hand.AddCard(card);

            // UIの演出が終わるまで「待機」する
            await _uiHandler.PlayDrawAnimationAsync(new DrawEventData
            {
                cardObj = card,
                drawIndex = i
            }, ct);

            // 次のドローまでの短い余韻
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: ct);
            card.CardStateChange(CardStateName.CardWaitState);
        }
    }

    /// <summary>
    /// 手札を全て捨て札へ送る
    /// </summary>
    /// <returns></returns>
    public async UniTask DiscardAllAsync(CancellationToken ct)
    {
        foreach (var card in BattleContext.Hand.Cards)
        {
            card.CardStateChange(CardStateName.CardIdleState);
            BattleContext.Discard.AddCard(card);
            await card.MoveToDiscard();
            await _uiHandler.PlayDiscardAnimationAsync(new DiscardEventData
            {
                cardObj = card
            }, ct);
        }
        BattleContext.Hand.Clear();
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
