public class DefenseEffect : StatusEffect
{
    public DefenseEffect(EffectData data, int stacks, IBattleUnit owner)
        : base(data, stacks, owner) { }

    public override void OnTurnStart() { }
    public override void OnTurnEnd()
    {
    }
}