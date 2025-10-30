using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Status")]
public class EnemyStatusEffect : EnemyEffect
{
    public StatusEffectData effectData;
    public override void Apply(BattleContext context, IEnemyUnit enemy, BattleUnit target)
    {
        target.AddEffect(effectData, amount);
    }
}