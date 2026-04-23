using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TimelineIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI actionName;
    [SerializeField] private GameObject highlightEffect;

    private int _enemyId;

    public void Setup(IBattleEvent battleEvent)
    {
        if (battleEvent.Type == EventType.Player)
        {
            iconImage.sprite = battleEvent.Hero.playerEventIcon;
            AddPreviewEffect();
            ChangeHighlight(false);
        }
        else if (battleEvent.Type == EventType.Enemy)
        {
            _enemyId = battleEvent.EnemyId;
            iconImage.sprite = battleEvent.Enemy.EventIcon;
            actionName.text = battleEvent.ActionName;
            ChangeHighlight(false);
        }
    }

    public void AddPreviewEffect()
    {
        gameObject.AddComponent<TimelinePreviewEffect>();
    }

    public void ConfirmedIcon()
    {
        var script = GetComponent<TimelinePreviewEffect>();
        script.enabled = false;
    }

    public void ChangeHighlight(bool active)
    {
        highlightEffect.SetActive(active);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EnemyUIEventChannel.OnEventRaised.Invoke(new EnemyUIEventData
        {
            EnemyId = _enemyId,
            Type = EnemyUIEventType.Highlight
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EnemyUIEventChannel.OnEventRaised.Invoke(new EnemyUIEventData
        {
            EnemyId = _enemyId,
            Type = EnemyUIEventType.Unhighlight
        });
    }
}