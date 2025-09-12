using System.Collections.Generic;

public class CardBlockEffect : ICardEffect
{
    public CardEffectTarget TargetType => CardEffectTarget.Self;
    private int _amount;
    public CardEffectType EffectType => CardEffectType.ApplyBlock;

    public CardBlockEffect(int amount)
    {
        _amount = amount;
    }

    public void Apply(CardContext context)
    {
        context.User.ApplyBlock(_amount);
    }
}