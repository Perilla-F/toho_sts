using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/HasBuff")]
public class HasBuffCondition : Condition
{
    public EffectData EffectData;
    public override bool Check(IBattleContext context, EnemyUnit enemy)
    {
        return enemy.HasStatus(EffectData);
    }
}