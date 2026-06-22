using System;
using System.Collections.Generic;

public class Hand : IHand
{
    public List<ICardObj> _cards;
    List<ICardObj> IHand.Cards => _cards;
    IReadOnlyList<IReadOnlyCardObj> IReadOnlyHand.Cards => _cards;

    public Hand()
    {
        _cards = new List<ICardObj>();
    }

    public void AddCard(ICardObj card)
    {
        _cards.Add(card);
    }

    public void RemoveCard(ICardObj card)
    {
        _cards.Remove(card);
    }

    public void Clear()
    {
        _cards.Clear();
    }

}
