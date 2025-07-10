using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardWaitState : CardStateBase
{

    // カードのデフォルトの重なり位置
    public int defaultSiblingIndex;
    //カードのデフォルトの位置
    Vector2 defaultPosition;


    public CardWaitState(CardBehaviour behaviour) : base(behaviour)
    {
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        behaviour.transform.DOScale(Vector3.one * 1.1f, 0.1f);
        behaviour.defaultSiblingIndex = behaviour.transform.GetSiblingIndex();
        // 一番上に表示する
        behaviour.transform.SetAsLastSibling();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        behaviour.transform.DOScale(Vector3.one, 0.1f);
        behaviour.transform.SetSiblingIndex(behaviour.defaultSiblingIndex);
    }

}
