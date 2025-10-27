using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Status")]
public class EnemyStatusEffect : EnemyEffect
{
    public StatusEffectData effectData;
    public override void Apply(BattleContext context, EnemyUnit enemy, BattleUnit target)
    {
        target.AddEffect(effectData, amount);
    }
}