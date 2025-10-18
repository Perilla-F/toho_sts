using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCell : CellBehavior
{
    public MultiStepEvent AssignedEvent;

    public override void OnPlayerEnter()
    {
        if (AssignedEvent == null)
        {
            Debug.LogWarning("EventCell にイベントが設定されていません。");
            return;
        }

        MapManager.Instance.SetLastEvent(AssignedEvent.EventId);
        EventManager.Instance.StartEvent(AssignedEvent);
    }
}
