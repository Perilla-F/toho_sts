using System.Collections.Generic;

public class CardDamageEffect : ICardEffect
{
    public CardEffectTarget TargetType { get; }
    private int _amount;
    public CardEffectType EffectType => CardEffectType.Damage;

    public CardDamageEffect(int amount, CardEffectTarget targetType)
    {
        TargetType = targetType;
        _amount = amount;
    }

    public void Apply(CardContext context)
    {
        List<IBattleUnit> Targets = context.Enemies;
        foreach (var target in Targets)
        {
            target.TakeDamage(_amount);
        }
    }
}