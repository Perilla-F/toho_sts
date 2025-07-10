using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSetUpState : CardStateBase
{
    public CardSetUpState(CardBehaviour behaviour) : base(behaviour)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("CardSetUpState OnEnter");
        behaviour.ChangeState(behaviour.IdleState);
    }
}
