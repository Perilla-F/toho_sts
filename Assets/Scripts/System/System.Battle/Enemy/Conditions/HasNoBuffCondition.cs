using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/HasNoBuff")]
public class HasNoBuffCondition : Condition
{
    public EffectData EffectData;
    public override bool Check(BattleContext context, EnemyUnit enemy)
    {
        return !enemy.HasStatus(EffectData);
    }
}