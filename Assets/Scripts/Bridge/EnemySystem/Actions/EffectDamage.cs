using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Damage")]
public class EffectDamage : EnemyEffectData
{
    public int amount;
    public override void Apply(IBattleContext context, IBattleUnit self)
    {
        context.Hero.TakeDamage(amount);
    }
}