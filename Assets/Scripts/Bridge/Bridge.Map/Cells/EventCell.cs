using System;
using UnityEngine;

public class EventCell : CellBehavior
{
    public MultiStepEvent AssignedEvent;
    public event Action<MultiStepEvent> OnEnter;

    public override void OnPlayerEnter()
    {
        if (AssignedEvent == null)
        {
            Debug.LogWarning("EventCell にイベントが設定されていません。");
            return;
        }

        OnEnter?.Invoke(AssignedEvent);
    }
}
