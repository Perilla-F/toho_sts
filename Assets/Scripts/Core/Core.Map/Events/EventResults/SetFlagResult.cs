using UnityEngine;

[CreateAssetMenu(menuName = "Events/Result/SetFlagResult")]
public class SetFlagResult : EventResult
{
    public string flagName;
    public bool value = true;

    public override void Apply(IGameManager game, IFlagManager flagManager)
    {
        if (value)
            flagManager.SetFlag(flagName);
        else
            flagManager.RemoveFlag(flagName);
    }
}
