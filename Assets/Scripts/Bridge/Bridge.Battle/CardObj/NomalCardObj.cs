using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalCardObj : CardObj
{

    public SourceCard SourceCard;
    public CardData CardData;
    public Mana Mana;
    Dictionary<CardEffectType, ICardEffectExecutor> _effectExecutors;
    private int _changedManaCost = 0;

    public NomalCardObj(SourceCard source, ResourceRegistry resourceRegistry, Mana mana) : base(source, resourceRegistry)
    {
        CardData = source.Data;
        this.Mana = mana;

        _effectExecutors = new Dictionary<CardEffectType, ICardEffectExecutor>
    {
        { CardEffectType.Damage, new DamageEffectExecutor() },
        { CardEffectType.ApplyBlock, new BlockEffectExecutor() },
        { CardEffectType.Draw, new DrawEffectExecutor() },
    };
    }

    public bool Use(CardContext context)
    {
        foreach (var cost in SourceCard.Data.Costs)
        {
            var res = ResourceRegistry.Get(cost.Type);
            if (res == null || res.CurrentMana < cost.Amount)
                return false; // どれか足りなければ中断
        }

        foreach (var cost in CardData.Costs)
        {
            ResourceRegistry.Get(cost.Type)?.TryConsume(cost.Amount);
        }

        foreach (var effect in CardData.CardEffects)
        {
            if (_effectExecutors.TryGetValue(effect.Type, out var executor))
            {
                executor.Execute(effect, context);
            }
        }
        return true;
    }
}
