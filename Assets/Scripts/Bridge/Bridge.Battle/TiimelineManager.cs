using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager
{
    public float CurrentTime { get; private set; }

    private SortedSet<BattleAction> actionQueue = new();

    public void Enqueue(BattleAction action)
    {
        actionQueue.Add(action);
    }

    public void AdvanceTime(float amount)
    {
        CurrentTime += amount;
        CheckActions();
    }

    public BattleAction DequeueNext()
    {
        if (actionQueue.Count == 0) return null;
        var next = actionQueue.Min;
        actionQueue.Remove(next);
        return next;
    }

    private void CheckActions()
    {
        foreach (var ac in actionQueue.ToList())
        {
            if (CurrentTime >= ac.scheduledTime)
            {
                ac.Execute();
                actionQueue.Remove(ac);
            }
        }
    }

    // Reset や巻き戻し用
    public void ResetTime()
    {
        CurrentTime = 0f;
        actionQueue.Clear();
    }
}
