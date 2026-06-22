using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class NomalCardObj : CardObj
{
    public int ChangedManaCost = 0;
    public int ChangedDelay = 0;

    public NomalCardObj(SourceCard source, ResourceRegistry resourceRegistry) : base(source, resourceRegistry)
    {
    }

    public async override UniTask Use(CardContext context)
    {
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

        // 演出の開始を待機する準備
        var tcs = new UniTaskCompletionSource();

        // 演出発火（通知）
        BattleEventBus.BattleEventAsync.OnCardUsed?.Invoke(context, tcs);

        // 演出が終わるまで待つ
        await tcs.Task;
    }

    public override bool Useable()
    {
        foreach (var cost in Source.Data.Costs)
        {
            var res = ResourceRegistry.Get(cost.Type);
            if (res == null || res.CurrentResource < cost.Amount)
            {
                Debug.Log("Miss Using");
                return false; // どれか足りなければ中断
            }
        }

        return true;
    }
}
