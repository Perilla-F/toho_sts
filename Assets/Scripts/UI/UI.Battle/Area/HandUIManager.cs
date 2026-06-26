using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;

public class HandUIManager
{
    private BattleViewRoot view;
    private IReadOnlyBattleContext _context;

    private ICardPoolProvider pool;

    private Dictionary<ICardObj, BattleCard> _cardDictionary = new();

    public HandUIManager(BattleViewRoot view, IReadOnlyBattleContext context)
    {
        this.view = view;
        _context = context;

        BattleEventBus.Card.OnCardDrawn += HandleCardDraw;
        BattleEventBus.Card.RestoreAllCards += RestoreAllCards;
    }

    public void Setup(ICardPoolProvider pool)
    {
        this.pool = pool;
    }

    public void CreateCardUI(ICardObj cardData, IReadOnlyBattleContext context, CancellationToken ct)
    {
        var card = pool.GetCard();// card(IPoolableCard)をBattleCardにキャスト
        if (card is BattleCard mono)
        {
            mono.transform.SetParent(view.HandView.transform, false);
            mono.transform.localScale = Vector3.one;
            card.BindCard(cardData, context);
            RegisterCard(cardData, mono);
        }
    }

    public BattleCard GetCardUI(ICardObj cardObj)
    {
        return _cardDictionary.TryGetValue(cardObj, out var card) ? card : null;
    }

    private void RegisterCard(ICardObj cardObj, BattleCard card) => _cardDictionary[cardObj] = card;
    private void UnregisterCard(ICardObj cardObj) => _cardDictionary.Remove(cardObj);

    private void HandleCardDraw(ICardObj cardData, IReadOnlyBattleContext context, CancellationToken ct)
    {
        OnCardDraw(cardData, context, ct).Forget();
    }

    private async UniTaskVoid OnCardDraw(ICardObj cardData, IReadOnlyBattleContext context, CancellationToken ct)
    {
        CreateCardUI(cardData, context, ct);
        var card = GetCardUI(cardData);
        await PlayDrawAnimationAsync(card, ct);
    }

    private async UniTask PlayDrawAnimationAsync(BattleCard card, CancellationToken ct)
    {
        card.transform.SetParent(view.HandView.transform, false);
        card.transform.localScale = Vector3.zero;

        card.transform.position = view.DeckView.GetTransform().position;

        view.HandView.AddCard(card);
        card.ChangeState(new CardBusyState(card));
        await view.HandView.ArrangeCards(ct);
    }

    public async UniTask DiscardCardAsync(ICardObj card, CancellationToken ct)
    {
        var cardUI = GetCardUI(card);
        if (cardUI == null) return; // UIが存在しないなら何もしない

        // 1. UIの管理リストから除外 (これが確実に呼ばれる必要がある)
        view.HandView.Discard(cardUI);

        // 2. アニメーションを確実に待機
        await PlayDiscardAnimationAsync(cardUI, ct);

        // 3. 辞書から消す
        UnregisterCard(card);

        // 4. プールに返す
        pool.ReturnCard(cardUI);

        BattleEventBus.View.OnChangedDiscardCount?.Invoke();

        await view.HandView.ArrangeCards(ct);
    }

    private async UniTask PlayDiscardAnimationAsync(BattleCard card, CancellationToken ct)
    {
        var discardView = view.DiscardView?.GetTransform();
        var targetPos = discardView != null ? discardView.position : Vector3.zero;

        var moveTask = card.transform.DOMove(targetPos, 0.3f)
            .SetEase(Ease.OutCubic)
            .WithCancellation(ct);

        var scaleTask = card.transform.DOScale(Vector3.zero, 0.3f)
            .WithCancellation(ct);

        await UniTask.WhenAll(moveTask, scaleTask);
    }

    private void RestoreAllCards()
    {
        foreach (var card in view.HandView.GetCards())
        {
            card.ChangeState(new CardRestState(card));
        }
    }

    private void OnDestroy()
    {
        BattleEventBus.Card.OnCardDrawn -= HandleCardDraw;
        BattleEventBus.Card.RestoreAllCards -= RestoreAllCards;
    }
}