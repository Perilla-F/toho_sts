using System.Collections;
using System;

public abstract class BattleEvent : IBattleEvent
{
    public int Time { get; private set; }
    public EventType Type { get; private set; }
    public int Priority { get; private set; } // 同時刻処理用（例: プレイヤー>ボス>雑魚）
    public int Order { get; set; }  // 雑魚の順番
    public bool IsFinished { get; protected set; }

    public BattleEvent(int scheduledTime, int priority = 0)
    {
        Time = scheduledTime;
        Priority = priority;
        IsFinished = false;
    }

    public abstract void Execute(BattleContext context);

}