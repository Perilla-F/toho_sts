using System.Collections;
using System;

public abstract class BattleEvent : IComparable<BattleEvent>
{
    public int ScheduledTime { get; private set; }
    public int Priority { get; private set; } // 同時刻処理用（例: プレイヤー>ボス>雑魚）

    public BattleEvent(int scheduledTime, int priority = 0)
    {
        ScheduledTime = scheduledTime;
        Priority = priority;
    }

    public abstract void Execute(IBattleContext context);

    // 時系列 + 優先度でソートできる
    public int CompareTo(BattleEvent other)
    {
        int timeCompare = ScheduledTime.CompareTo(other.ScheduledTime);
        if (timeCompare != 0) return timeCompare;
        return Priority.CompareTo(other.Priority);
    }
}