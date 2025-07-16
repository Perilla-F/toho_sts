using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCell : CellBehaviour
{
    public EventBase assignedEvent;
    public override void OnPlayerEnter()
    {
        MapGenerator.Instance.StartEvent(assignedEvent);
    }
}
