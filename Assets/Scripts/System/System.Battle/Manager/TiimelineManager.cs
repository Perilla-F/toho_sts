using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager : ITimelineManager
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

    public BattleEvent PopNextEvent()
    {
        if (_events.Count == 0) return null;
        var next = _events[0];
        _events.RemoveAt(0);
        return next;
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
    /// 残りイベントを一斉消化
    /// </summary>
    public IEnumerator FlushAll(BattleContext context)
    {
        while (_events.Count > 0)
        {
            var e = PopNextEvent();
            e.Execute(context);
            CurrentTime = e.Time;

            // ここでアニメーション終了を待つ
            yield return new WaitUntil(() => e.IsFinished);

            // ちょっと間を置く演出
            yield return new WaitForSeconds(0.3f);
        }
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

}
