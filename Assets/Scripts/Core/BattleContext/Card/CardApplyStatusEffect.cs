using System.Collections.Generic;

public class CardApplyStatusEffect : ICardEffect
{
    public CardEffectTarget TargetType { get; private set; }
    private StatusEffectData _data;
    private int _amount;
    public CardEffectType EffectType => CardEffectType.Status;

    public CardApplyStatusEffect(StatusEffectData effect, int amount, CardEffectTarget targetType)
    {
        _data = effect;
        _amount = amount;
        TargetType = targetType;
    }

    public void Apply(CardContext context)
    {
        foreach (var target in context.Targets)
        {
            target.AddEffect(_data, _amount);
        }
    }
}