using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardWaitState : CardStateBase
{

    // カードのデフォルトの重なり位置
    public int DefaultSiblingIndex;
    //カードのデフォルトの位置
    private Vector2 _defaultPosition;


    public CardWaitState(CardBehavior behaviour) : base(behaviour)
    {
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        _behaviour.transform.DOScale(Vector3.one * 1.1f, 0.1f);
        _behaviour.DefaultSiblingIndex = _behaviour.transform.GetSiblingIndex();
        // 一番上に表示する
        _behaviour.transform.SetAsLastSibling();
        // タイムライン上にアイコンを載せる
        PlayerController.Instance.SetPreviewDelay(_behaviour.CardObj.Source.Data.Delay);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        _behaviour.transform.DOScale(Vector3.one, 0.1f);
        _behaviour.transform.SetSiblingIndex(_behaviour.DefaultSiblingIndex);
        // タイムライン上からアイコンを外す
        PlayerController.Instance.ClearPreview();
    }

}
