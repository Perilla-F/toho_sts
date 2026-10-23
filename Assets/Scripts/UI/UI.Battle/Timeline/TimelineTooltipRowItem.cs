using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TimelineTooltipRowItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI rowText;

    private string keywordDescription;
    private TimelineSubTooltipController subTooltipController; // サブツールチップを管理する親

    public void Setup(string displayText, string value, string description)
    {
        if (rowText != null) rowText.text = displayText + value;
        keywordDescription = description;
        subTooltipController = GetComponent<TimelineSubTooltipController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // この行にマウスが乗ったら、サブツールチップに詳細を表示させる！
        if (subTooltipController != null)
        {
            // 自分の位置（またはマウス位置）を基準にサブツールチップを表示
            subTooltipController.ShowSubTooltip(keywordDescription);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 行からマウスが離れたとき（必要に応じてサブを隠す、または親側で制御する）
        if (subTooltipController != null)
        {
            subTooltipController.HideSubTooltip();
        }
    }
}