using System.Collections.Generic;
using UnityEngine;

public class EnemyUIEventListener
{
    private int _enemyId;
    private EnemyUI _ui;

    public void Initialize(int enemyId, EnemyUI ui)
    {
        _enemyId = enemyId;
        _ui = ui;

        EnemyUIEventChannel.OnEventRaised += HandleEventChanging;
        EnemyUIEventChannel.OnEventPlaned += HandleEventPlanning;
        BuffUIChannel.OnBuffAdded += HandleBuffAdd;
        BuffUIChannel.OnBuffUpdated += HandleBuffUpdate;
    }

    private void HandleEventChanging(EnemyUIEventData data)
    {
        if (data.EnemyId != _enemyId) return;

        switch (data.Type)
        {
            case EnemyUIEventType.Highlight:
                _ui.Highlight(true, data.Number);
                break;
            case EnemyUIEventType.Unhighlight:
                _ui.Highlight(false, data.Number);
                break;
        }
    }

    private void HandleEventPlanning(NormalAction[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            _ui.SetActionIcon(actions[i].actionType, actions[i].ScheduledTime, i);
        }
    }

    private void HandleBuffAdd(StatusEffect data)
    {
        _ui.SetBuffIcon(data);
    }

    private void HandleBuffUpdate(StatusEffect data)
    {
        _ui.UpdateBuffIcon(data);
    }

    public void OnMouseEnter()
    {
        EnemyUIEventChannel.OnEventRaised.Invoke(new EnemyUIEventData
        {
            EnemyId = _enemyId,
            Type = EnemyUIEventType.Highlight
        });
    }

    public void OnMouseExit()
    {
        EnemyUIEventChannel.OnEventRaised.Invoke(new EnemyUIEventData
        {
            EnemyId = _enemyId,
            Type = EnemyUIEventType.Unhighlight
        });
    }
}
