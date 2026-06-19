using System;
using System.Collections;
using System.Collections.Generic;

public class DiscardArea : IDiscardArea
{
    private List<ICardObj> _discardedCards;

    public event Action<int> OnChangedDiscardCount;

    public int Count => _discardedCards.Count;

    public DiscardArea()
    {
        _discardedCards = new List<ICardObj>();
    }

    public void AddCard(ICardObj cardObj)
    {
        _discardedCards.Add(cardObj);
        //        OnChangedDiscardCount?.Invoke(_discardedCards.Count);
    }

    public void ResetDiscardPile()
    {
        _discardedCards.Clear();
        OnChangedDiscardCount?.Invoke(_discardedCards.Count);
    }

    public List<ICardObj> GetDiscardPile()
    {
        return _discardedCards;
    }

    public void ShuffleBackInto(BattleDeck deck)
    {
        foreach (var card in _discardedCards)
        {
            deck.AddCard(card);
        }
        deck.Shuffle();
        ResetDiscardPile();
    }
}
