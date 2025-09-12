using System.Collections.Generic;

public class CardHealEffect : ICardEffect
{
    public CardEffectTarget TargetType => CardEffectTarget.Self;
    private int _amount;
    public CardEffectType EffectType => CardEffectType.Heal;

    public CardHealEffect(int amount)
    {
        _amount = amount;
    }

    public void Apply(CardContext context)
    {
        context.User.Heal(_amount);
    }
}