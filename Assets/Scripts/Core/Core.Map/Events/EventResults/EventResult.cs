using UnityEngine;

public abstract class EventResult : ScriptableObject
{
    public abstract void Apply(GameContext context, IFlagManager flagManager);
}
