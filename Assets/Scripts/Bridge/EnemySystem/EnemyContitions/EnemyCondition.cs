using UnityEngine;

public abstract class EnemyCondition : ScriptableObject
{
    public abstract bool IsSatisfied(IBattleContext context, IBattleUnit self);
}