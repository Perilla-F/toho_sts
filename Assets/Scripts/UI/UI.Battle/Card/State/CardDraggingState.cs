using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDraggingState : CardStateBase
{
    public CardDraggingState(CardBehaviour behaviour) : base(behaviour)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("DraggingState OnEnter");
    }

    public override void OnUpdate()
    {
        behaviour.transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(1))
        {
            behaviour.ResetPos();
            behaviour.ChangeState(behaviour.WaitState);
        }

        if (behaviour.transform.localPosition.y > 100)
        {
            behaviour.ChangeState(behaviour.SelectedState);
        }
    }
}
