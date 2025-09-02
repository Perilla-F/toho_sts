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
        _behaviour.transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(1))
        {
            _behaviour.ResetPos();
            _behaviour.ChangeState(_behaviour.WaitState);
        }

        if (_behaviour.transform.localPosition.y > 100)
        {
            _behaviour.ChangeState(_behaviour.SelectedState);
        }
    }
}
