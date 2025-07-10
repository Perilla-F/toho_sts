using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineView : MonoBehaviour
{
    private TimelineManager timelineManager;

    public void Bind(TimelineManager timelineManager)
    {
        this.timelineManager = timelineManager;
    }

}
