using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSetUpState : CardStateBase
{
    public CardSetUpState(BattleCard behaviour) : base(behaviour)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("CardSetUpState OnEnter");
        _behaviour.ChangeState(_behaviour.IdleState);
    }
}
