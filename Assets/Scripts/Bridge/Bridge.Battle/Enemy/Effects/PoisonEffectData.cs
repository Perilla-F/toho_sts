using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Poison")]
public class PoisonEffectData : EffectData
{
    public override void Apply(IBattleUnit self, IBattleContext context, IBattleUnit target, int amount)
    {
        target.AddEffect(this, amount);
    }
}