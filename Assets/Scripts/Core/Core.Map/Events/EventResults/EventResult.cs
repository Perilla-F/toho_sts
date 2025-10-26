using UnityEngine;

public abstract class EventResult : ScriptableObject
{
    public abstract void Apply(IGameManager game, IFlagManager flagManager);
}
