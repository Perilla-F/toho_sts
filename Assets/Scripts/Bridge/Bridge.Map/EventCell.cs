using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCell : CellBehaviour
{
    public string eventId;
    public override void OnPlayerEnter()
    {
        MapGenerator.Instance.StartEvent(eventId);
    }
}
