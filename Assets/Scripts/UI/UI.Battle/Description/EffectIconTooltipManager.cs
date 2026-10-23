using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class EffectIconTooltipManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private GameObject tooltipPanel; // ツールチップのUIオブジェクト

    private string cachedTitle;
    private string cachedDescription;
    private bool isHoveringIcon = false;
    private bool isHoveringTooltip = false;

    /// <summary>
    /// バフアイコン側からツールチップの内容を登録する
    /// </summary>
    public void SetTooltipContent(string title, string description)
    {
        cachedTitle = title;
        cachedDescription = description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringIcon = true;
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHoveringIcon = false;
        Hide();
    }

    private void ShowTooltip()
    {
        if (tooltipPanel != null)
        {
            if (_nameText != null) _nameText.text = cachedTitle;
            if (_descriptionText != null) _descriptionText.text = cachedDescription;
            tooltipPanel.SetActive(true);
        }
    }

    private void Hide()
    {
        tooltipPanel.SetActive(false);
    }

}