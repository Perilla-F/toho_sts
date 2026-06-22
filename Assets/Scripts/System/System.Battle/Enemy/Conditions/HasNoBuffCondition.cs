using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/HasNoBuff")]
public class HasNoBuffCondition : Condition
{
    public EffectData EffectData;
    public override bool Check(IBattleContext context, EnemyUnit enemy)
    {
        return !enemy.HasStatus(EffectData);
    }
}