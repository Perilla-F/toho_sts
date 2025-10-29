using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalCardObj : CardObj
{
    public int ChangedManaCost = 0;
    public int ChangedDelay = 0;

    public NomalCardObj(SourceCard source, ResourceRegistry resourceRegistry) : base(source, resourceRegistry)
    {
    }

    public bool Use(CardContext context)
    {
        foreach (var cost in Source.Data.Costs)
        {
            var res = ResourceRegistry.Get(cost.Type);
            if (res == null || res.CurrentResource < cost.Amount)
                return false; // どれか足りなければ中断
        }

        foreach (var cost in Source.Data.Costs)
        {
            ResourceRegistry.Get(cost.Type)?.TryConsume(cost.Amount);
        }

        Source.Data.ApplyEffects(context);
        if (Source.Data.CardEffectTarget == CardEffectTarget.Enemy ||
            Source.Data.CardEffectTarget == CardEffectTarget.AllEnemies ||
            Source.Data.CardEffectTarget == CardEffectTarget.Random)
        {
            context.User.Attack();  // プレイヤーアニメーション
            foreach (var target in context.Targets)
            {
                target.Hit();   // 敵アニメーション
            }
        }
        return true;
    }
}
