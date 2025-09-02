using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineView : MonoBehaviour
{
    private TimelineManager _timelineManager;

    public void Bind(TimelineManager timelineManager)
    {
        this._timelineManager = timelineManager;
    }

}
