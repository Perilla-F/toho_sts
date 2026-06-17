using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardRestState : CardStateBase
{
    public CardRestState(BattleCard owner) : base(owner)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("RestState OnEnter");
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        _owner.CardHover();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        _owner.HoverCancel();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
    }
}
