using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/HasBuff")]
public class HasBuffCondition : Condition
{
    public EffectData EffectData;
    public override bool Check(BattleContext context, EnemyUnit enemy)
    {
        return enemy.HasStatus(EffectData);
    }
}