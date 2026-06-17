using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class EnemyAction : ScriptableObject
{
    public string actionName;
    public BattleUnit Self;
    public EnemyActionTarget target;
    public int ScheduledTime;
    public bool IsCanceled;

    public abstract UniTask Execute(IBattleContext context, IEnemyUnit enemy);
}