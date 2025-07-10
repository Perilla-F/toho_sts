using System.Collections;
using System.Collections.Generic;

public class DiscardArea : IDiscardArea
{
    private List<CardObj> discardedCards = new List<CardObj>();
    public void AddCard(CardObj cardObj)
    {
        discardedCards.Add(cardObj);
    }

    public void ResetDiscardPile()
    {
        discardedCards.Clear();
    }

    public List<CardObj> GetDiscardPile()
    {
        return discardedCards;
    }

    public void ShuffleBackInto(BattleDeck deck)
    {
        foreach (var card in discardedCards)
        {
            deck.AddCard(card);
        }
        deck.Shuffle();
    }
}
