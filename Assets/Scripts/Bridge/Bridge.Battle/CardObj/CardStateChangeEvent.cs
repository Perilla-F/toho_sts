using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStateChangeEvent : ICardStateChangeEvent
{
    public CardObj CardObj { get; }
    public CardStateName StateName { get; }

    public CardStateChangeEvent(CardObj cardObj, CardStateName stateName)
    {
        CardObj = cardObj;
        StateName = stateName;
    }

    object ICardStateChangeEvent.Source => CardObj;
}
