using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/SetFlagResult")]
public class SetFlagResult : EventResult
{
    public string FlagToSet;
    public string FlagToRemove;

    public override void Apply(IGameContext context, IFlagManager flags, EventOption option)
    {
        if (!string.IsNullOrEmpty(FlagToSet))
            flags.SetFlag(FlagToSet);
        if (!string.IsNullOrEmpty(FlagToRemove))
            flags.RemoveFlag(FlagToRemove);
    }
}
