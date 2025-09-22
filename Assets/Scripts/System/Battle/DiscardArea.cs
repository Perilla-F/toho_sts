using System.Collections;
using System.Collections.Generic;

public class DiscardArea : IDiscardArea
{
    private List<CardObj> _discardedCards = new List<CardObj>();
    public void AddCard(CardObj cardObj)
    {
        _discardedCards.Add(cardObj);
    }

    public void ResetDiscardPile()
    {
        _discardedCards.Clear();
    }

    public List<CardObj> GetDiscardPile()
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
    }
}
