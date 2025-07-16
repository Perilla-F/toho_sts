using UnityEngine;

public abstract class EventBase : ScriptableObject
{
    public string eventId;  // "forward"など
    public string displayName;
    public string description;

    public abstract void Execute();
}