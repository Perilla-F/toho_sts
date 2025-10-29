using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CardObj : ICardObj
{
    public Func<UniTask> MoveToHandAsync { get; private set; }
    public Func<UniTask> MoveToDiscardAsync { get; private set; }
    public SourceCard Source;
    public readonly ResourceRegistry ResourceRegistry;
    public int Delay;
    public ICardView cardView;

    public CardObj(SourceCard source, ResourceRegistry resourceRegistry)
    {
        Source = source;
        ResourceRegistry = resourceRegistry;
        Delay = source.Data.Delay;
    }

    public void BindMoveToHand(Func<UniTask> moveFunc)
    {
        MoveToHandAsync = moveFunc;
    }

    public async UniTask MoveCardAsync()
    {
        if (MoveToHandAsync != null)
        {
            await MoveToHandAsync();
        }
    }

    public void BindMoveToDiscard(Func<UniTask> moveFunc)
    {
        MoveToDiscardAsync = moveFunc;
    }

    public async UniTask MoveDisCardAsync()
    {
        if (MoveToHandAsync != null)
        {
            await MoveToDiscardAsync();
        }
    }

    public void CardStateChange(CardStateName stateName)
    {
        EventBus<CardStateChangeEvent>.Publish(new CardStateChangeEvent(this, stateName));
    }

    /// <summary>
    /// CardObj→SourceCard変換
    /// </summary>
    /// <returns></returns>
    public SourceCard GetSource()
    {
        return Source;
    }

    public virtual async UniTask Use()
    {
        await UniTask.CompletedTask;
    }
}