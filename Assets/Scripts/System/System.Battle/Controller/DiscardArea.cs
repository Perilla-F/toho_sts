using System;
using System.Collections;
using System.Collections.Generic;

public class DiscardArea : IDiscardArea
{
    private List<ICardObj> _discardedCards;


    public int Count => _discardedCards.Count;

    public DiscardArea()
    {
        _discardedCards = new List<ICardObj>();
    }

    public void AddCard(ICardObj cardObj)
    {
        _discardedCards.Add(cardObj);
    }

    public void ResetDiscardPile()
    {
        _discardedCards.Clear();
        BattleEventBus.View.OnChangedDiscardCount?.Invoke();
    }

    public List<ICardObj> GetDiscardPile()
    {
        return _discardedCards;
    }

    /// <summary>
    /// 捨て札をデッキに戻してシャッフル
    /// </summary>
    /// <param name="deck"></param>
    public void ShuffleBackInto(IBattleDeck deck)
    {
        foreach (var card in _discardedCards)
        {
            deck.AddCard(card);
        }
        deck.Shuffle();
        ResetDiscardPile();
    }
}
