using Cysharp.Threading.Tasks;
using UnityEngine;

public class FearEffect : StatusEffect
{
    public FearEffect(EffectData data, int stacks, IBattleUnit owner)
        : base(data, stacks, owner) { }

    public override void OnTurnStart() { }

    public override void OnTurnEnd()
    {
        RemoveStacks(1);
    }
}