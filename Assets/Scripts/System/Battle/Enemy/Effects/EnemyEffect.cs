using UnityEngine;

public abstract class EnemyEffect : ScriptableObject
{
    public string effectName;
    public int amount;
    public abstract void Apply(BattleContext context, IEnemyUnit enemy, BattleUnit target);
}