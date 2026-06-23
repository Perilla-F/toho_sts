using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/HasBuff")]
public class HasBuffCondition : Condition
{
    public EffectData EffectData;
    public override bool Check(IReadOnlyBattleContext context, IReadOnlyEnemyUnit enemy)
    {
        return enemy.HasStatus(EffectData.EffectId);
    }
}