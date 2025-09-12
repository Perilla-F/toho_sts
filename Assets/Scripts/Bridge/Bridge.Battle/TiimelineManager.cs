using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager
{
    private SortedSet<BattleEvent> events = new SortedSet<BattleEvent>();

    public void AddEvent(BattleEvent e)
    {
        events.Add(e);
    }

    public void ProcessEventsUntilTurnEnd(IBattleContext context, int currentTurnEndTime)
    {
        while (events.Count > 0 && events.Min.ScheduledTime <= currentTurnEndTime)
        {
            var e = events.Min;
            events.Remove(e);
            e.Execute(context);
        }
    }

    /// <summary>
    /// 残りイベントを一斉消化
    /// </summary>
    public void FlushAll(IBattleContext context)
    {
        while (events.Count > 0)
        {
            var e = events.Min;
            events.Remove(e);
            e.Execute(context);
        }
    }

}
