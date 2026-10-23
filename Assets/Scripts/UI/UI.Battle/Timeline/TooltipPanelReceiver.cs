using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipPanelReceiver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // どのコントローラーに通知するか
    private TimelineTooltipManager controller;

    public void Initialize(TimelineTooltipManager controller)
    {
        this.controller = controller;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (controller != null) controller.OnTooltipPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (controller != null) controller.OnTooltipPointerExit();
    }
}