using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Fear")]
public class FearEffectData : EffectData
{
    public override void Apply(IBattleUnit self, IBattleContext context, IBattleUnit target, int amount)
    {
        target.AddEffect(this, amount);
    }
}