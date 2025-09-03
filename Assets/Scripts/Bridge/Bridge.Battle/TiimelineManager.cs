using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager
{
    public float CurrentTime { get; private set; }

    private SortedSet<BattleAction> _actionQueue = new();

    public void Enqueue(BattleAction action)
    {
        _actionQueue.Add(action);
    }

    public void AdvanceTime(float amount)
    {
        CurrentTime += amount;
        CheckActions();
    }

    public BattleAction DequeueNext()
    {
        if (_actionQueue.Count == 0) return null;
        var next = _actionQueue.Min;
        _actionQueue.Remove(next);
        return next;
    }

    private void CheckActions()
    {
        foreach (var ac in _actionQueue.ToList())
        {
            if (CurrentTime >= ac.Action.ScheduledTime)
            {
                ac.Execute();
                _actionQueue.Remove(ac);
            }
        }
    }

    // Reset や巻き戻し用
    public void ResetTime()
    {
        CurrentTime = 0f;
        _actionQueue.Clear();
    }
}
