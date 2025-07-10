using System.Collections.Generic;

public class Hand : IHand
{
    private List<CardObj> cards = new List<CardObj>();
    public IReadOnlyList<CardObj> Cards => cards;

    public Hand()
    {
    }

    public void AddCard(CardObj card)
    {
        cards.Add(card);
    }

    public void RemoveCard(CardObj card)
    {
        cards.Remove(card);
    }

    public void Clear()
    {
        cards.Clear();
    }

}
