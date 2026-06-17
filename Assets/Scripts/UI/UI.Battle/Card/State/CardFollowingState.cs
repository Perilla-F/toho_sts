using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardFollowingState : CardStateBase
{
    public CardFollowingState(BattleCard owner) : base(owner)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("FollowingState OnEnter");
        BezierArrows.Instance.Hide();
    }

    public override void OnUpdate()
    {
        Vector2 mousePos = _owner.GetMouseCanvasPos();
        _owner.transform.localPosition = Vector2.Lerp(_owner.transform.localPosition, mousePos, 0.3f);
        if (mousePos.y > _owner.thresholdY)
        {
            _owner.ChangeState(new CardTargetingState(_owner, false));
        }
        if (Input.GetMouseButtonDown(1))
        {
            _owner.ResetPos();
        }
    }

}
