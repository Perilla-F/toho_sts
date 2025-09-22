using UnityEngine;

public abstract class EventBase
{
    public abstract string EventId { get; }  // "forward"など
    public abstract string DisplayName { get; }
    public abstract string Description { get; }

    public abstract void Execute();
}