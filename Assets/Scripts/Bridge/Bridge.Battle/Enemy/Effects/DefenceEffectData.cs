using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Defence")]
public class DefenceEffectData : EffectData
{
    public override void Apply(IBattleUnit self, IBattleContext context, IBattleUnit target, int amount)
    {
        target.AddEffect(this, amount);
    }
}