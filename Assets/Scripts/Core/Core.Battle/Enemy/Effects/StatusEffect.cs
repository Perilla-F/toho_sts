using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Status")]
public class EnemyStatusEffect : EnemyEffect
{
    public StatusEffectData effectData;
    public override void Apply(IBattleContext context, IEnemyUnit enemy, BattleUnit target)
    {
        target.AddEffect(effectData, amount);
    }
}