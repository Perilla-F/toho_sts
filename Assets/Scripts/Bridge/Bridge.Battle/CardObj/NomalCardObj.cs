using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalCardObj : CardObj
{

    public SourceCard sourceCard;
    public CardData cardData;
    public Mana mana;
    Dictionary<CardEffectType, ICardEffectExecutor> _effectExecutors;
    private int ChangedManaCost = 0;

    public NomalCardObj(SourceCard source, ResourceRegistry resourceRegistry, Mana mana) : base(source, resourceRegistry)
    {
        cardData = source.data;
        this.mana = mana;

        _effectExecutors = new Dictionary<CardEffectType, ICardEffectExecutor>
    {
        { CardEffectType.Damage, new DamageEffectExecutor() },
        { CardEffectType.ApplyBlock, new BlockEffectExecutor() },
        { CardEffectType.Draw, new DrawEffectExecutor() },
    };
    }

    public bool Use(CardContext context)
    {
        foreach (var cost in sourceCard.data.costs)
        {
            var res = resourceRegistry.Get(cost.Type);
            if (res == null || res.CurrentMana < cost.amount)
                return false; // どれか足りなければ中断
        }

        foreach (var cost in cardData.costs)
        {
            resourceRegistry.Get(cost.Type)?.TryConsume(cost.amount);
        }

        foreach (var effect in cardData.cardEffects)
        {
            if (_effectExecutors.TryGetValue(effect.type, out var executor))
            {
                executor.Execute(effect, context);
            }
        }
        return true;
    }
}
