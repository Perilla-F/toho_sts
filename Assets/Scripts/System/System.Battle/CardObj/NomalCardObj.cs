using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class NomalCardObj : CardObj
{
    public int ChangedManaCost = 0;
    public int ChangedDelay = 0;

    public NomalCardObj(SourceCard source, ResourceRegistry resourceRegistry, EnemyTargetSelector selector) : base(source, resourceRegistry, selector)
    {
    }

    public async override UniTask Use(ICardContext context)
    {
        foreach (var cost in Source.Data.Costs)
        {
            ResourceRegistry.Get(cost.Type)?.TryConsume(cost.Amount);
        }

        Source.Data.ApplyEffects(Selector, context);

        // 演出の開始を待機する準備
        var tcs = new UniTaskCompletionSource();

        // 演出発火（通知）
        BattleEventBus.BattleEventAsync.OnCardUsed?.Invoke(Source.Data, context, tcs);

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
