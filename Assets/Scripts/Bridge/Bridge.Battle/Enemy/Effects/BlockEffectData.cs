using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Block")]
public class BlockEffectData : EffectData
{
    public override void Apply(int id, IBattleContext context, IBattleUnit target, int amount)
    {
        target.ApplyBlock(amount);
    }
}