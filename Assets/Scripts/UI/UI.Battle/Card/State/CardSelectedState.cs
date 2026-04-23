using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardSelectedState : CardStateBase
{
    public CardSelectedState(BattleCard behaviour) : base(behaviour)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Selected onEnter");

        _behaviour.transform.DOLocalMove(Vector3.zero, 0.1f);
        BezierArrows.Instance.Show();
        BezierArrows.Instance.SetColor(Color.gray);
    }

    public override void OnUpdate()
    {
        BezierArrows.Instance.SetOriginPos(_behaviour.transform.position);
        BezierArrows.Instance.SetTopPos(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            _behaviour.ResetPos();
            _behaviour.ChangeState(_behaviour.WaitState);
        }
    }
}