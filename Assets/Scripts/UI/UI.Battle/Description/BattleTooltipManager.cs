using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class BattleTooltipManager : MonoBehaviour
{

    public static BattleTooltipManager Instance;

    [SerializeField] private RectTransform _tooltipCanvas;
    [SerializeField] private GameObject _effectItemPrefab; // 上部のリスト用
    [SerializeField] private TextMeshProUGUI _descriptionText; // 下部の詳細用

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Hide();
    }

    public void Show(List<EffectDescription> effects)
    {
        // 1. HeaderPanelの子要素をクリアして再生成
        // 2. 詳細説明文を結合して表示
        string detailText = string.Join("\n\n", effects.Select(e => $"<b>{e.Name}</b>\n{e.Description}"));
        _descriptionText.text = detailText;

        gameObject.SetActive(true);

        UpdateTooltipPosition();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateTooltipPosition()
    {
        RectTransform tooltipRect = GetComponent<RectTransform>();
        Camera cam = GetComponentInParent<Canvas>().worldCamera;
        RectTransform parentRect = tooltipRect.parent as RectTransform;

        // 1. マウス位置をCanvas上のローカル座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect, Input.mousePosition, cam, out Vector2 localPos);

        // 2. ツールチップのサイズを取得
        float width = tooltipRect.rect.width;
        float height = tooltipRect.rect.height;

        // 3. 画面の端判定（Screen Space で判定）
        // マウス位置から画面の右端までの余白が、ツールチップの幅より狭いか？
        bool isRightEdge = Input.mousePosition.x + width + 50 > Screen.width;
        bool isTopEdge = Input.mousePosition.y + height + 50 > Screen.height;

        // 4. ピボットとオフセットを動的に決定
        // 右端なら左側に、上端なら下側に表示する
        float pivotX = isRightEdge ? 1f : 0f;
        float pivotY = isTopEdge ? 1f : 0f;

        // オフセット（マウスとの距離）
        float offsetX = isRightEdge ? 0f : 0f;
        float offsetY = isTopEdge ? 0f : 0f;

        // 適用
        tooltipRect.pivot = new Vector2(pivotX, pivotY);
        tooltipRect.anchoredPosition = localPos + new Vector2(offsetX, offsetY);
    }

}