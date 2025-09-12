using UnityEngine;

public abstract class EnemyEffectData : ScriptableObject
{
    public abstract void Apply(IBattleContext context, IBattleUnit self);
}