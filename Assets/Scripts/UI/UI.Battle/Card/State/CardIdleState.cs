using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIdleState : CardStateBase
{
    public CardIdleState(BattleCard behaviour) : base(behaviour)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("IdleState OnEnter");
    }
}
