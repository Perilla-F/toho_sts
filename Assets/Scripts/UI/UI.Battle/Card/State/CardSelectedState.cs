using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardSelectedState : CardStateBase
{
    public CardSelectedState(CardBehaviour behaviour) : base(behaviour)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Selected onEnter");

        behaviour.transform.DOLocalMove(Vector3.zero, 0.1f);
        BezierArrows.Instance.Show();
        BezierArrows.Instance.SetColor(Color.gray);
    }

    public override void OnUpdate()
    {
        BezierArrows.Instance.SetOriginPos(behaviour.transform.position);
        BezierArrows.Instance.SetTopPos(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            behaviour.ResetPos();
            behaviour.ChangeState(behaviour.WaitState);
        }
    }
}