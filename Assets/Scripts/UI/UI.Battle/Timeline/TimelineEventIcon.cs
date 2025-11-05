using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimelineEventIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private int _enemyId;
    private EnemyUIEventChannel _uiChannel;
    [SerializeField] private Image iconImage;

    public void Initialize(int enemyId, Sprite sprite, EnemyUIEventChannel uiChannel)
    {
        _enemyId = enemyId;
        _uiChannel = uiChannel;
        if (iconImage != null)
            iconImage.sprite = sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _uiChannel?.Raise(new EnemyUIEventData
        {
            EnemyId = _enemyId,
            Type = EnemyUIEventType.Highlight
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _uiChannel?.Raise(new EnemyUIEventData
        {
            EnemyId = _enemyId,
            Type = EnemyUIEventType.Unhighlight
        });
    }
}
