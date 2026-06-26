using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Defence")]
public class DefenseEffectData : EffectData
{
    public override void Apply(int id, IBattleContext context, IBattleUnit target, int amount)
    {
        target.AddEffect(this, amount);
    }
}