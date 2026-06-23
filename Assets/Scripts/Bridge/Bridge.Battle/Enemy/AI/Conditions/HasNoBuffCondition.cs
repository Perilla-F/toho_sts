using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/HasNoBuff")]
public class HasNoBuffCondition : Condition
{
    public EffectData EffectData;
    public override bool Check(IReadOnlyBattleContext context, IReadOnlyEnemyUnit enemy)
    {
        return !enemy.HasStatus(EffectData.EffectId);
    }
}