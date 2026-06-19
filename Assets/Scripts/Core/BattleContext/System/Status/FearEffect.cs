using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Fear")]
public class FearEffect : StatusEffect
{
    public FearEffect(EffectData data, int stacks, BattleUnit owner)
        : base(data, stacks, owner) { }

    public override async UniTask OnTurnStart() { }

    public override async UniTask OnTurnEnd()
    {
        RemoveStacks(1);
    }
}