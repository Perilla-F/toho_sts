using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/SetFlagResult")]
public class SetFlagResult : EventResult
{

    public override void Apply(IGameContext context, IFlagManager flags, EventOption option)
    {
        if (!string.IsNullOrEmpty(option.FlagToSet))
            flags.SetFlag(option.FlagToSet);
        if (!string.IsNullOrEmpty(option.FlagToRemove))
            flags.RemoveFlag(option.FlagToRemove);
    }
}
