using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Damage")]
public class DamageEffectData : EffectData
{
    public override void Apply(IBattleUnit self, IBattleContext context, IBattleUnit target, int amount)
    {
        var fainalDamage = context.BattleSystem.CalculateDamage(self, target, amount);
        target.TakeDamageAsync(amount);
    }
}