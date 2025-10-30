using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimelineEventIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private IEnemyActionEvent _event;
    [SerializeField] private Image iconImage;

    public void Initialize(IEnemyActionEvent e)
    {
        _event = e;
        iconImage.sprite = e.Enemy.EventIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _event?.ReferenceEnemy?.ShowActionHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _event?.ReferenceEnemy?.ShowActionHighlight(false);
    }
}
