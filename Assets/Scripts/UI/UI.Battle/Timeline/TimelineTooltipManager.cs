using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TimelineTooltipManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltipPanel; // ツールチップのUIオブジェクト
    [SerializeField] private Transform rowContainer;
    [SerializeField] private GameObject rowPrefab;

    private List<EffectDescription> _effects;
    private bool isHoveringIcon = false;
    private bool isHoveringTooltip = false;
    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (tooltipPanel != null)
        {
            // ツールチップパネル側にも受信用のスクリプトを自動で結びつける（またはアタッチしておく）
            var receiver = tooltipPanel.GetComponent<TooltipPanelReceiver>();
            if (receiver == null) receiver = tooltipPanel.AddComponent<TooltipPanelReceiver>();
            receiver.Initialize(this);

            tooltipPanel.SetActive(false);
        }
    }

    /// <summary>
    /// タイムラインバー側からツールチップの内容を登録する
    /// </summary>
    public void SetTooltipContent(List<EffectDescription> effects)
    {
        _effects = effects;
        CreateRows();
    }

    public void CreateRows()
    {
        foreach (var effect in _effects)
        {
            var row = Instantiate(rowPrefab, rowContainer);
            var rowItem = row.GetComponent<TimelineTooltipRowItem>();
            rowItem.Setup(effect.Name, effect.Value, effect.Description);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringIcon = true;
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHoveringIcon = false;
        TryHideWithDelay();
    }

    public void OnTooltipPointerEnter()
    {
        isHoveringTooltip = true;
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }
    }

    public void OnTooltipPointerExit()
    {
        isHoveringTooltip = false;
        TryHideWithDelay();
    }

    private void ShowTooltip()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(true);
        }
    }

    private void TryHideWithDelay()
    {
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        hideCoroutine = StartCoroutine(HideDelayRoutine());
    }

    private IEnumerator HideDelayRoutine()
    {
        // 1フレーム待つことで、「アイコンから出てツールチップに入る瞬間」の隙間をカバー
        yield return null;

        // アイコンの上にも、ツールチップの上にもどちらにもいない場合のみ消す
        if (!isHoveringIcon && !isHoveringTooltip)
        {
            if (tooltipPanel != null)
            {
                tooltipPanel.SetActive(false);
            }
        }
    }

    // public void UpdateTooltipPosition()
    // {
    //     RectTransform tooltipRect = GetComponent<RectTransform>();
    //     Camera cam = GetComponentInParent<Canvas>().worldCamera;
    //     RectTransform parentRect = tooltipRect.parent as RectTransform;

    //     // 1. マウス位置をCanvas上のローカル座標に変換
    //     RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //         parentRect, Input.mousePosition, cam, out Vector2 localPos);

    //     // 2. ツールチップのサイズを取得
    //     float width = tooltipRect.rect.width;
    //     float height = tooltipRect.rect.height;

    //     // 3. 画面の端判定（Screen Space で判定）
    //     // マウス位置から画面の右端までの余白が、ツールチップの幅より狭いか？
    //     bool isRightEdge = Input.mousePosition.x + width + 50 > Screen.width;
    //     bool isTopEdge = Input.mousePosition.y + height + 50 > Screen.height;

    //     // 4. ピボットとオフセットを動的に決定
    //     // 右端なら左側に、上端なら下側に表示する
    //     float pivotX = isRightEdge ? 1f : 0f;
    //     float pivotY = isTopEdge ? 1f : 0f;

    //     // オフセット（マウスとの距離）
    //     float offsetX = isRightEdge ? 0f : 0f;
    //     float offsetY = isTopEdge ? 0f : 0f;

    //     // 適用
    //     tooltipRect.pivot = new Vector2(pivotX, pivotY);
    //     tooltipRect.anchoredPosition = localPos + new Vector2(offsetX, offsetY);
    // }

}