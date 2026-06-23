using UnityEngine;

public abstract class Condition : ScriptableObject
{
    public abstract bool Check(IReadOnlyBattleContext context, IReadOnlyEnemyUnit enemy);
}