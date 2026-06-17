using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDraggingState : CardStateBase
{
    public override bool Dragging => true;

    public CardDraggingState(BattleCard owner) : base(owner)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("DraggingState OnEnter");
    }

    public override void OnUpdate()
    {
        Vector2 mousePos = _owner.GetMouseCanvasPos();
        _owner.transform.localPosition = Vector2.Lerp(_owner.transform.localPosition, mousePos, 0.3f);
        if (mousePos.y > _owner.thresholdY)
        {
            _owner.ChangeState(new CardTargetingState(_owner, true));
        }
        if (Input.GetMouseButtonDown(1))
        {
            _owner.ResetPos();
        }
    }
}
