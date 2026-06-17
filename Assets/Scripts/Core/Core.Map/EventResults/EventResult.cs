using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class EventResult : ScriptableObject
{
    public abstract UniTask Apply(IGameContext context, IFlagManager flags, EventOption option);
}
