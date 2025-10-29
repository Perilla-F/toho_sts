using System;
using System.Collections.Generic;

public class Hand : IHand
{
    public List<CardObj> Cards = new List<CardObj>();

    public event Action OnChangedHand;

    public Hand()
    {
    }

    public void AddCard(CardObj card)
    {
        Cards.Add(card);
        OnChangedHand?.Invoke();
    }

    public void RemoveCard(CardObj card)
    {
        Cards.Remove(card);
        OnChangedHand?.Invoke();
    }

    public void Clear()
    {
        Cards.Clear();
        OnChangedHand?.Invoke();
    }

}
