using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardUIComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CardVisual visual;

    private CardData _data;
    private IReadOnlyCardObj _card;

    // 状況次第でクリックを無効化できるようにするフラグ
    public bool IsInspectable { get; set; } = true;

    // バトル外（デッキ確認画面など）で使う初期化
    public void SetupForDeckMenu(CardData data)
    {
        // バフなしの素の状態で見た目を更新
        visual.UpdateVisual(data);
        _data = data;
    }

    // バトル中（山札・捨て札確認画面など）で使う初期化
    public void SetupForBattleViewer(IReadOnlyCardObj cardObj, IReadOnlyBattleContext battleContext)
    {
        // バトル中のバフ（強化など）を反映して見た目を更新
        // ※必要に応じてバトル中のコンテキストを渡す
        visual.UpdateVisual(cardObj, battleContext);
        _data = cardObj.Source.Data;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // クリック無効状態なら何もしない
        if (!IsInspectable) return;

        // 詳細ポップアップマネージャーを呼び出して自分を表示する
        ServiceLocator.Get<ICardDetailPopupManager>().OpenPopup(_data);
    }

    /// <summary>
    /// マウスオーバー
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsInspectable) return;
        transform.DOScale(Vector3.one * 1.1f, 0.15f).SetEase(Ease.OutCubic);
    }

    /// <summary>
    /// マウスオーバー解除
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsInspectable) return;
        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutCubic);
    }
}