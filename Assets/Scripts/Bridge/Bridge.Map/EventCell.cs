using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCell : CellBehaviour
{
    public EventBase AssignedEvent;
    public override void OnPlayerEnter()
    {
        MapGenerator.Instance.StartEvent(AssignedEvent);
    }
}
