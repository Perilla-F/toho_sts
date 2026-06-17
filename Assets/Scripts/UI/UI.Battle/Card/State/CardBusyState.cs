using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBusyState : CardStateBase
{
    public override bool CanDrag => false;

    public CardBusyState(BattleCard owner) : base(owner)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("BusyState OnEnter");
    }
}
