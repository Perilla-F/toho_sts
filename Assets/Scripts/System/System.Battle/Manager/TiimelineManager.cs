using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager
{
    private List<BattleEvent> _events = new List<BattleEvent>();
    public int CurrentTime { get; set; }

    public void AddEvent(BattleEvent e)
    {
        _events.Add(e);
        SortEvents();
    }

    public void AdvanceToNextEvent()
    {
        if (_events.Count == 0) return;
        var next = _events.OrderBy(e => e.Time).First();
        CurrentTime = next.Time;
    }

    public IEnumerator PopNextEvent(BattleContext context)
    {
        var next = _events[0];
        _events.RemoveAt(0);
        //next.Execute(context);
        ExecuteAction(next, context);

        CurrentTime = next.Time;

        // ここでアニメーション終了を待つ
        yield return new WaitUntil(() => next.IsFinished);
        BattleEventBus.OnActionExecuted?.Invoke(next);

        // ちょっと間を置く演出
        yield return new WaitForSeconds(0.3f);
    }

    private void SortEvents()
    {
        _events.Sort((a, b) =>
        {
            int cmp = a.Time.CompareTo(b.Time);
            if (cmp != 0) return cmp;

            // Type優先度: Player(0) < Boss(1) < Enemy(2)
            cmp = a.Type.CompareTo(b.Type);
            if (cmp != 0) return cmp;

            // 雑魚の左から順
            return a.Order.CompareTo(b.Order);
        });
    }

    public int GetCurrentTime()
    {
        return CurrentTime;
    }

    public IReadOnlyList<IBattleEvent> GetUpcomingEvents()
    {
        return _events
            .OrderBy(e => e.Time)
            .ThenBy(e => e.Priority)
            .ThenBy(e => e.Order)
            .ToList();
    }

    public bool HasEvents()
    {
        return _events.Count > 0;
    }

    /// <summary>
    /// 次のイベントを引っ張る
    /// </summary>
    /// <returns></returns>
    public BattleEvent PeekNextEvent()
    {
        if (_events.Count == 0) return null;
        return _events[0];
    }

    /// <summary>
    /// タイムラインの構築が完了した時に呼び出されるメソッド
    /// </summary>
    public void OnTimelineBuilt()
    {
        // Bridge層のイベントを呼び出し、UI層に通知する
        BattleEventBus.OnActionsDecided?.Invoke(_events);
    }

    /// <summary>
    /// 規定時間に行動を実行するメソッド
    /// </summary>
    public void ExecuteAction(BattleEvent info, BattleContext context)
    {
        // 実際のロジック（ダメージ計算など）を実行...
        info.Execute(context);

        // Bridge層のイベントを呼び出し、UI層に演出を通知する
        BattleEventBus.OnActionExecuted?.Invoke(info);
    }

}
