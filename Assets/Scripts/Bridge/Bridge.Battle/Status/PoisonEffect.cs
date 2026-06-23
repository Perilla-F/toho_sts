using Cysharp.Threading.Tasks;
using UnityEngine;

public class PoisonEffect : StatusEffect
{
    public PoisonEffect(EffectData data, int stacks, IBattleUnit owner)
        : base(data, stacks, owner) { }

    public override void OnTurnStart() { }

    public override void OnTurnEnd()
    {
        int damage = Stacks;
        Owner.TakeDamageAsync(damage);
        RemoveStacks(1);
    }
}