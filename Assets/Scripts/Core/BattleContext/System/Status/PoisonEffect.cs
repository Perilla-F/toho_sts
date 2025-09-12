public class PoisonEffect : StatusEffect
{
    public PoisonEffect(StatusEffectData data, int stacks, IBattleUnit owner)
        : base(data, stacks, owner) { }

    public override void OnTurnStart() { }

    public override void OnTurnEnd()
    {
        int damage = Stacks * Data.baseValue;
        Owner.TakeDamage(damage);
        RemoveStacks(1);
    }
}