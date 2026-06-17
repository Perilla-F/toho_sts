using System;
using System.Collections.Generic;

public class Hand : IHand
{
    public List<ICardObj> Cards = new List<ICardObj>();

    public Hand()
    {
    }

    public void AddCard(ICardObj card)
    {
        Cards.Add(card);
    }

    public void RemoveCard(ICardObj card)
    {
        Cards.Remove(card);
    }

    public void Clear()
    {
        Cards.Clear();
    }

}
