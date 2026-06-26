using System.Collections;
using System.Threading.Tasks;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class BattleEvent
{
    public Sprite EventIcon;
    public int Time { get; private set; }
    public EventType Type { get; set; }
    public String ActionName { get; set; }
    public int Priority { get; private set; } // 同時刻処理用（例: プレイヤー>ボス>雑魚）
    public int Order { get; set; }  // 雑魚の順番
    public bool IsFinished { get; protected set; }

    public BattleEvent(Sprite eventIcon, int scheduledTime, int priority = 0)
    {
        EventIcon = eventIcon;
        Time = scheduledTime;
        Priority = priority;
        IsFinished = false;
    }

    public virtual UniTask Execute(IBattleContext context) => UniTask.CompletedTask;

}