using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;
using System;

public class HandUIManager
{
    private BattleViewRoot view;

    private ICardPoolProvider pool;

    public event Action OnCompleteDiscardAnimation;

    public HandUIManager(BattleViewRoot view)
    {
        this.view = view;

        BattleEventBus.RestoreAllCards += RestoreAllCards;
    }

    public void Setup(ICardPoolProvider pool)
    {
        this.pool = pool;
    }

    public BattleCard CreateCardUI(ICardObj cardData)
    {
        var card = pool.GetCard();// card(IPoolableCard)をBattleCardにキャスト
        if (card is BattleCard mono)
        {
            mono.transform.SetParent(view.HandView.transform, false);
            mono.transform.localScale = Vector3.one;
            card.BindCard(cardData);
            return mono;
        }
        return null;
    }

    public async UniTask PlayDrawAnimationAsync(BattleCard card, CancellationToken ct)
    {
        card.transform.SetParent(view.HandView.transform, false);
        card.transform.localScale = Vector3.zero;

        card.transform.position = view.DeckView.GetTransform().position;

        view.HandView.AddCard(card);
        card.ChangeState(new CardBusyState(card));
        await view.HandView.ArrangeCards(ct);
    }

    public async UniTask PlayDiscardAnimationAsync(BattleCard card, CancellationToken ct)
    {
        view.HandView.Discard(card);

        var arrangeTask = view.HandView.ArrangeCards(ct);

        var moveTask = card.transform
            .DOMove(view.DiscardAreaView.GetTransform().position, 0.3f)
            .SetEase(Ease.OutCubic)
            .WithCancellation(ct);

        var scaleTask = card.transform
            .DOScale(Vector3.zero, 0.3f)
            .WithCancellation(ct);

        await UniTask.WhenAll(moveTask, scaleTask);

        pool.ReturnCard(card);
        OnCompleteDiscardAnimation?.Invoke();

        await arrangeTask;
    }

    public void RestoreAllCards()
    {
        foreach (var card in view.HandView.GetCards())
        {
            card.ChangeState(new CardRestState(card));
        }
    }

    private void OnDestroy()
    {
        BattleEventBus.RestoreAllCards -= RestoreAllCards;
    }
}