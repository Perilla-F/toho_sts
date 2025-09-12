using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalCardObj : CardObj
{
    public Mana Mana;
    private int _changedManaCost = 0;

    public NomalCardObj(SourceCard source, ResourceRegistry resourceRegistry, Mana mana) : base(source, resourceRegistry)
    {
        Mana = mana;
    }

    public bool Use(CardContext context)
    {
        foreach (var cost in Source.Data.Costs)
        {
            var res = ResourceRegistry.Get(cost.Type);
            if (res == null || res.CurrentMana < cost.Amount)
                return false; // どれか足りなければ中断
        }

        foreach (var cost in Source.Data.Costs)
        {
            ResourceRegistry.Get(cost.Type)?.TryConsume(cost.Amount);
        }

        foreach (var effect in Source.Data.CardEffects)
        {
            effect.Apply(context);
        }
        return true;
    }
}
