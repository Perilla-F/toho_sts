using UnityEngine;

public abstract class Condition : ScriptableObject
{
    public abstract bool Check(IBattleContext context, EnemyUnit enemy);
}