using UnityEngine;
using TMPro;

public class TimelineSubTooltipController : MonoBehaviour
{
    [SerializeField] private GameObject subTooltipPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;

    int offset = 0;

    private void Start()
    {
        if (subTooltipPanel != null) subTooltipPanel.SetActive(false);
    }

    public void ShowSubTooltip(string description)
    {
        if (descriptionText != null) descriptionText.text = description;

        if (subTooltipPanel != null)
        {
            // メインツールチップ（または対象の行）の右側にサブパネルの位置を合わせる
            subTooltipPanel.SetActive(true);
        }
    }

    public void HideSubTooltip()
    {
        if (subTooltipPanel != null)
        {
            subTooltipPanel.SetActive(false);
        }
    }
}