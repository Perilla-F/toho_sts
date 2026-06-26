using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class EnemyAction : ScriptableObject
{
    public string actionName;
    public int ScheduledTime;
    public bool IsCanceled;

    public abstract UniTask Execute(int id, IBattleContext context);
}