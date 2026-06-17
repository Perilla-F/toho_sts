using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardStateBase
{
    protected BattleCard _owner;
    public virtual bool CanDrag => true;
    public virtual bool Dragging => false;

    public CardStateBase(BattleCard owner)
    {
        _owner = owner;
    }

    public virtual void OnEnter() { }

    public virtual void OnUpdate() { }

    public virtual void OnExit() { }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }

    public virtual void OnBeginDrag(PointerEventData eventData) { }

}
