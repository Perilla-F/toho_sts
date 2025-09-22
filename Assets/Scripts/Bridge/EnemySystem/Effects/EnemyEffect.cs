using UnityEngine;

public abstract class EnemyEffect : ScriptableObject
{
    public string effectName;
    public int amount;
    public abstract void Apply(IBattleContext context, EnemyUnit enemy, IBattleUnit target);
}