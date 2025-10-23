using System.Collections.Generic;

public class Hand : IHand
{
    private List<CardObj> _cards = new List<CardObj>();
    public IReadOnlyList<CardObj> Cards => _cards;

    public Hand()
    {
    }

    public void AddCard(CardObj card)
    {
        _cards.Add(card);
    }

    public void RemoveCard(CardObj card)
    {
        _cards.Remove(card);
    }

    public void Clear()
    {
        _cards.Clear();
    }

}
