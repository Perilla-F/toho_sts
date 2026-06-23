using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour, IBattleSystem
{
    private IEnemyManager _enemyManager;
    private IHeroUnit _hero;
    private TimelineManager _timelineManager;
    private IBattleContext _context;
    private IAudioManager _audioService;


    private CancellationToken _ct;

    public void Setup(IBattleContext context, HeroUnit heroUnit, IEnemyManager enemy, TimelineManager timelineManager, IAudioManager audioService)
    {
        _context = context;
        _hero = heroUnit;
        _enemyManager = enemy;
        _timelineManager = timelineManager;

        _audioService = audioService;

        //audioService?.PlayBGM("battle_theme", true);

        _ct = this.GetCancellationTokenOnDestroy();
    }

    /// <summary>
    /// 1ターンの敵の行動をタイムラインに登録
    /// </summary>
    public void AddActionToTimeline()
    {
        foreach (var enemy in _enemyManager.GetAllEnemies())
        {
            var actionDatas = enemy.PlanTurn(_context);
            foreach (var actionData in actionDatas)
            {
                int priority = enemy.EnemyType == EnemyType.Normal ? 2 : 1;
                var enemyEvent = new EnemyActionEvent(_hero, enemy, actionData, actionData.ScheduledTime, priority);
                _timelineManager.AddEvent(enemyEvent);
            }
        }
        _timelineManager.OnTimelineBuilt(true);
    }

    /// <summary>
    /// タイムライン処理
    /// </summary>
    /// <param name="targetTime"></param>
    /// <returns></returns>
    public async UniTask ProcessUntilTime(int targetTime)
    {
        // 目標時刻に達する前のイベントをすべて実行
        while (_timelineManager.HasEventsUntil(targetTime))
        {
            await _timelineManager.ExecuteNextEventAsync(_context);
        }

        // イベントがない「空白の時間」を埋める（タイムラインの針を目標まで進める）
        _timelineManager.CurrentTime = targetTime;
        BattleEventBus.BattleEventAsync.OnUpdateTime?.Invoke();
        Debug.Log($"Time is {_timelineManager.CurrentTime} Count!!");
    }

    /// <summary>
    /// 最終ダメージ計算
    /// </summary>
    /// <param name="user"></param>
    /// <param name="target"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public int CalculateDamage(IBattleUnit user, IBattleUnit target, int value)
    {
        var damage = user.StatusCount("strength") + value;
        if (target.Effects.Exists(e => e.Data.EffectId == "broken")) damage += damage / 2;
        return damage;
    }

    public void ExecuteAttack(IBattleUnit attacker, IBattleUnit defender, int baseDamage)
    {
        // 1. 計算：ダメージ量を確定させる
        int damageToDeal = DamageCalculator.CalculateDamage(attacker, defender, baseDamage);

        // 2. 防御：防御コンポーネントに消費させる
        int finalDamage = defender.DefenseComponent.Consume(damageToDeal);

        // 3. HP適用：残ったダメージだけHPに与える
        if (finalDamage > 0)
        {
            defender.HPResource.LoseHP(finalDamage);
        }

        Debug.Log($"最終ダメージ: {damageToDeal}, 防御後の被ダメージ: {finalDamage}");
    }

    public async UniTask DrawMultipleAsync(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_context.Deck.IsEmpty())
            {
                _context.Discard.ShuffleBackInto(_context.Deck);
            }
            var card = _context.Deck.Draw();
            _context.Hand.AddCard(card);

            BattleEventBus.Card.OnCardDrawn?.Invoke(card, _context, _ct);
            BattleEventBus.View.OnChangedDeckCount?.Invoke();

            await UniTask.Delay(TimeSpan.FromSeconds(0.15f), cancellationToken: _ct);
        }
    }

    /// <summary>
    /// バトル終了処理
    /// </summary>
    public void EndBattle(CancellationToken ct)
    {
        SceneManager.LoadScene("MapScene");
    }

}
