using System.Collections.Generic;

public class CardSimpleBlockEffect : ICardEffect
{
    public CardEffectTarget TargetType => CardEffectTarget.Self;
    private int _amount;
    public CardEffectType EffectType => CardEffectType.SimpleBlock;

    public CardSimpleBlockEffect(int amount)
    {
        _amount = amount;
    }

    public void Apply(CardContext context)
    {
        context.User.ApplySimpleBlock(_amount);
    }
}