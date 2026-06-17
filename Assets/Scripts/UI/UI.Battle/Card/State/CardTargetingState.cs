using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardTargetingState : CardStateBase
{
    private bool _isEntryFromDrag;
    private Vector3 cardPos;

    private bool targetable;

    public CardTargetingState(BattleCard owner, bool isFromDrag) : base(owner)
    {
        _isEntryFromDrag = isFromDrag;
        cardPos = new Vector3(0, _owner.thresholdY, 0);

        targetable = _owner.Card.IsSingleTarget;
    }

    public override void OnEnter()
    {
        Debug.Log("TargetingState OnEnter");
        _owner.transform.DOLocalMove(cardPos, 0.1f).SetEase(Ease.OutCubic);
        if (targetable)
        {
            BezierArrows.Instance.Show();
            BezierArrows.Instance.SetColor(Color.gray);
        }
    }

    public override void OnUpdate()
    {
        Vector2 mousePos = _owner.GetMouseCanvasPos();
        BezierArrows.Instance.SetOriginPos(_owner.transform.localPosition);
        BezierArrows.Instance.SetTopPos(_owner.GetMouseCanvasPos());
        if (mousePos.y < _owner.thresholdY)
        {
            if (_isEntryFromDrag)
                _owner.ChangeState(new CardDraggingState(_owner));
            else
                _owner.ChangeState(new CardFollowingState(_owner));
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            _owner.ResetPos();
        }
    }

    public override void OnExit()
    {
        BezierArrows.Instance.Hide();
    }
}