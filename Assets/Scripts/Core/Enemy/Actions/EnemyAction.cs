using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction : ScriptableObject
{
    public string actionName;
    public Sprite icon;
    public BattleUnit Self;
    public EnemyActionTarget target;
    public int ScheduledTime;
    public bool IsCanceled;

    public abstract void Execute(BattleContext context, EnemyUnit enemy);
}