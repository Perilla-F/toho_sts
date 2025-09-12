using System.Collections.Generic;

public class CardDrawEffect : ICardEffect
{
    public CardEffectTarget TargetType => CardEffectTarget.None;
    private int _amount;
    public CardEffectType EffectType => CardEffectType.Draw;

    public CardDrawEffect(int amount)
    {
        _amount = amount;
    }

    public void Apply(CardContext context)
    {
        context.BattleSystem.Draw(_amount);
    }
}