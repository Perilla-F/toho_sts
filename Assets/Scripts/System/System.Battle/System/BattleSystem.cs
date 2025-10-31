using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool _isProcessingEvents;
    private BattlePhase phase = BattlePhase.TurnStart;
    public BattleContext BattleContext { get; private set; }
    private IAudioManager AudioService;

    public int TurnCount { get; private set; } = 1;

    public event Action<int> OnTurnStart;

    public void Init(EnemyManager enemy, PlayerController player)
    {
        _enemyManager = enemy;
        _player = player;
    }

    public void Setup(BattleContext context, HeroUnit heroUnit, GameManager gameManager, TimelineManager timelineManager, IAudioManager audioService)
    {
        BattleContext = context;
        Hero = heroUnit;
        _timelineManager = timelineManager;

        this.gameManager = gameManager;
        AudioService = audioService;

        StartCoroutine(BattleLoop());

        audioService?.PlayBGM("battle_theme", true);
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
                int priority = 2;
                if (enemy.EnemyType != EnemyType.Normal)
                {
                    priority = 1;
                }
                var enemyEvent = new EnemyActionEvent(enemy, actionData, actionData.ScheduledTime, priority);
                _timelineManager.AddEvent(enemyEvent);
            }
        }
    }

    /// <summary>
    /// バトルプロセス
    /// </summary>
    /// <returns></returns>
    private IEnumerator BattleLoop()
    {
        while (true)
        {
            yield return StartCoroutine(TurnStart());
            yield return StartCoroutine(PlayerSelectPhase());
            yield return StartCoroutine(TurnEnd());
        }
    }

    /// <summary>
    /// ターン開始処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator TurnStart()
    {
        phase = BattlePhase.TurnStart;
        Debug.Log("=== Turn Start ===");

        OnTurnStart?.Invoke(TurnCount);

        AddActionToTimeline();
        Draw(Hero.DrawCount).Forget();

        yield return null;
    }

    /// <summary>
    /// プレイヤー行動選択中
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerSelectPhase()
    {
        phase = BattlePhase.PlayerSelect;
        Debug.Log("=== Player Select Phase ===");

        _player.BeginSelection();

        bool endTurn = false;

        while (!endTurn)
        {
            // プレイヤーが行動を選択した場合
            if (_player.HasChosenAction)
            {
                var card = _player.ChosenCard;
                int actionTime = _timelineManager.CurrentTime + card.Delay;

                _timelineManager.AddEvent(new PlayerActionEvent(
                    _player.ChosenCard,
                    _timelineManager.CurrentTime + _player.ChosenCard.Delay
                ));

                _player.ConfirmAction();

                // 行動発動時刻まで時間を進める（途中の敵行動などを処理）
                yield return StartCoroutine(ProcessUntilTime(actionTime));

                // 行動発動後 → 再び選択可能に
                _player.BeginSelection();
            }

            // ターンエンドが押された場合
            if (_player.TurnEndRequested)
            {
                endTurn = true;
            }

            yield return null;
        }

        // 残りイベントを全部処理
        yield return StartCoroutine(ProcessAllEvents());
    }

    /// <summary>
    /// イベント処理
    /// </summary>
    /// <param name="targetTime"></param>
    /// <returns></returns>
    private IEnumerator ProcessUntilTime(int targetTime)
    {
        phase = BattlePhase.TimelineRunning;
        while (_timelineManager.HasEvents() && _timelineManager.PeekNextEvent().Time <= targetTime)
        {
            var e = _timelineManager.PopNextEvent();
            _timelineManager.CurrentTime = e.Time;
            e.Execute(BattleContext);
            yield return new WaitUntil(() => e.IsFinished);
            yield return new WaitForSeconds(0.3f);
        }
    }

    /// <summary>
    /// 残りイベント全処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProcessAllEvents()
    {
        phase = BattlePhase.TimelineRunning;
        while (_timelineManager.HasEvents())
        {
            var e = _timelineManager.PopNextEvent();
            _timelineManager.CurrentTime = e.Time;
            e.Execute(null);
            yield return new WaitUntil(() => e.IsFinished);
            yield return new WaitForSeconds(0.3f);
        }
    }

    /// <summary>
    /// ターン終了処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator TurnEnd()
    {
        phase = BattlePhase.TurnEnd;
        Debug.Log("=== Turn End ===");

        MoveAllToDiscard().Forget();
        yield return new WaitForSeconds(1f);
        TurnCount++;
    }

    public async UniTask Draw(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (BattleContext.Deck.IsEmpty()) break;
            CardObj card = BattleContext.Deck.Draw();
            BattleContext.Hand.AddCard(card);
            if (i < count - 1)
            {
                card.MoveToHand().Forget();
                await UniTask.Delay(100);
            }
            else
            {
                await card.MoveToHand();
            }
            card.CardStateChange(CardStateName.CardWaitState);
        }
    }

    /// <summary>
    /// 手札を全て捨て札へ送る
    /// </summary>
    /// <returns></returns>
    public async UniTask MoveAllToDiscard()
    {
        foreach (var card in BattleContext.Hand.Cards)
        {
            card.CardStateChange(CardStateName.CardIdleState);
            BattleContext.Discard.AddCard(card);
            await card.MoveToDiscard();
        }
        BattleContext.Hand.Clear();
    }

    /// <summary>
    /// バトル終了処理
    /// </summary>
    public void EndBattle()
    {
        List<SourceCard> updatedDeck = BattleContext.Deck.GetDeckAsSourceCards();

        gameManager.UpdateDeckAfterBattle(updatedDeck);

        SceneManager.LoadScene("MapScene");
    }
}
