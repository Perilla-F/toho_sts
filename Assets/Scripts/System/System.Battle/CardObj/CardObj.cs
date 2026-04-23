using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CardObj : ICardObj
{
    public SourceCard Source { get; private set; }
    public readonly ResourceRegistry ResourceRegistry;
    private readonly PlayerController _controller;
    public int Delay;
    public ICardView _view;

    public CardObj(SourceCard source, ResourceRegistry resourceRegistry, PlayerController controller)
    {
        Source = source;
        ResourceRegistry = resourceRegistry;
        Delay = source.Data.Delay;
        _controller = controller;
    }

    public void BindView(ICardView cardView)
    {
        _view = cardView;
    }

    public async UniTask MoveToHand()
    {
        if (_view != null)
            await _view.MoveToHandAsync();
    }

    public async UniTask MoveToDiscard()
    {
        if (_view != null)
            await _view.MoveToDiscardAsync();
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

    public void SetPreviewDelay()
    {
        _controller.SetPreviewDelay(Source.Data.Delay);
    }

    public void ClearPreview()
    {
        _controller.ClearPreview();
    }
}