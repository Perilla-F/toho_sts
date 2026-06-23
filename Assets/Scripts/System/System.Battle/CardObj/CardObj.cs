using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class CardObj : ICardObj
{
    public SourceCard Source { get; private set; }
    public CardEffectTarget TargetType { get; private set; }
    public readonly ResourceRegistry ResourceRegistry;
    public readonly EnemyTargetSelector Selector;
    public int Delay { get; private set; }

    public CardObj(SourceCard source, ResourceRegistry resourceRegistry, EnemyTargetSelector selector)
    {
        Source = source;
        ResourceRegistry = resourceRegistry;
        Selector = selector;
        Delay = source.Data.Delay;
        TargetType = Source.Data.CardEffectTarget;
    }

    /// <summary>
    /// CardObj→SourceCard変換
    /// </summary>
    /// <returns></returns>
    public SourceCard GetSource()
    {
        return Source;
    }

    public virtual async UniTask Use(ICardContext context)
    {
        await UniTask.CompletedTask;
    }

    public virtual bool Useable()
    {
        return false;
    }

}