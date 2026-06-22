using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "Events/Results/HasFlag")]
public class HasFlagResult : EventResult
{
    public string flagName;
    public EventResult ifTrue;
    public EventResult ifFalse;

    public override void Apply(IGameContext context, IFlagManager flagManager, EventOption option)
    {
        bool has = flagManager.HasFlag(flagName);
        Debug.Log($"Condition: {flagName} = {has}");
        (has ? ifTrue : ifFalse)?.Apply(context, flagManager, option);
    }
}