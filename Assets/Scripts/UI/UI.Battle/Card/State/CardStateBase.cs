using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardStateBase
{
    protected CardBehaviour behaviour;

    public CardStateBase(CardBehaviour behaviour)
    {
        this.behaviour = behaviour;
    }

    public virtual void OnEnter() { }

    public virtual void OnUpdate() { }

    public virtual void OnExit() { }

    public virtual void OnClick() { }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }
}
