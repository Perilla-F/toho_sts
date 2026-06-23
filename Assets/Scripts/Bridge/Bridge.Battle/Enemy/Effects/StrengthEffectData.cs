using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Strength")]
public class StrengthEffectData : EffectData
{
    public override void Apply(IBattleUnit self, IBattleContext context, IBattleUnit target, int amount)
    {
        target.AddEffect(this, amount);
    }
}