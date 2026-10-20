using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class CardObj : ICardObj
{
    public SourceCard Source { get; protected set; }
    public CardEffectTarget TargetType { get; protected set; }
    public readonly EnemyTargetSelector Selector;
    public int Delay { get; protected set; }

    public CardObj(SourceCard source, EnemyTargetSelector selector)
    {
        Source = source;
        Selector = selector;
        Delay = source.Data.Delay;
        TargetType = Source.Data.CardEffectTarget;
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