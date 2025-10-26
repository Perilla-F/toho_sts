using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/Conditional")]
public class ConditionalResult : EventResult
{
    public string flagName;
    public EventResult ifTrue;
    public EventResult ifFalse;

    public override void Apply(IGameManager game, IFlagManager flagManager)
    {
        bool has = flagManager.HasFlag(flagName);
        Debug.Log($"Condition: {flagName} = {has}");
        (has ? ifTrue : ifFalse)?.Apply(game, flagManager);
    }
}