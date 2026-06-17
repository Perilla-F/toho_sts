using Cysharp.Threading.Tasks;

public class PoisonEffect : StatusEffect
{
    public PoisonEffect(StatusEffectData data, int stacks, BattleUnit owner)
        : base(data, stacks, owner) { }

    public override async UniTask OnTurnStart() { }

    public override async UniTask OnTurnEnd()
    {
        int damage = Stacks;
        await Owner.TakeDamageAsync(damage);
        RemoveStacks(1);
    }
}