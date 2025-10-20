using UnityEngine;

public class EnemyUIEventListener : MonoBehaviour
{
    private int _enemyId;
    private EnemyUI _ui;
    private EnemyUIEventChannel _channel;

    public void Initialize(int enemyId, EnemyUIEventChannel channel, EnemyUI ui)
    {
        _enemyId = enemyId;
        _channel = channel;
        _ui = ui;

        _channel.OnEventRaised += HandleEvent;
    }

    private void HandleEvent(EnemyUIEventData data)
    {
        if (data.EnemyId != _enemyId) return;

        switch (data.Type)
        {
            case EnemyUIEventType.ShowIntent:
                _ui.ShowIntentIcon(data.Icon);
                break;
            case EnemyUIEventType.Highlight:
                _ui.Highlight(true);
                break;
            case EnemyUIEventType.Unhighlight:
                _ui.Highlight(false);
                break;
        }
    }

    public void OnMouseEnter()
    {
        _channel.Raise(new EnemyUIEventData
        {
            EnemyId = _enemyId,
            Type = EnemyUIEventType.Highlight
        });
    }

    public void OnMouseExit()
    {
        _channel.Raise(new EnemyUIEventData
        {
            EnemyId = _enemyId,
            Type = EnemyUIEventType.Unhighlight
        });
    }
}
