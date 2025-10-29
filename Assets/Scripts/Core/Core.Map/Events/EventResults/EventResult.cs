using UnityEngine;

public abstract class EventResult : ScriptableObject
{
    public abstract void Apply(IGameContext context, IFlagManager flagManager);
}
