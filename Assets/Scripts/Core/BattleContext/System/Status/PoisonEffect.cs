public class PoisonEffect : StatusEffect
{
    public PoisonEffect(StatusEffectData data, int stacks, BattleUnit owner)
        : base(data, stacks, owner) { }

    public override void OnTurnStart() { }

    public override void OnTurnEnd()
    {
        int damage = Stacks;
        Owner.TakeDamage(damage);
        RemoveStacks(1);
    }
}