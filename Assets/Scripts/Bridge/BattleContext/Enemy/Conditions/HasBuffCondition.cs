using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/HasBuff")]
public class HasBuffCondition : Condition
{
    public EffectData EffectData;
    public override bool Check(IBattleContext context, IEnemyUnit enemy)
    {
        return enemy.HasStatus(EffectData);
    }
}