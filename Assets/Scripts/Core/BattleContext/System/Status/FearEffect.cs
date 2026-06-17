using Cysharp.Threading.Tasks;

public class FearEffect : StatusEffect
{
    public FearEffect(StatusEffectData data, int stacks, BattleUnit owner)
        : base(data, stacks, owner) { }

    public override async UniTask OnTurnStart() { }

    public override async UniTask OnTurnEnd()
    {
        RemoveStacks(1);
    }
}