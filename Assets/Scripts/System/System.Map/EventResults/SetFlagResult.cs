using UnityEngine;

[CreateAssetMenu(menuName = "Events/Result/SetFlagResult")]
public class SetFlagResult : EventResult
{
    public string flagName;
    public bool value = true;

    public override void Apply()
    {
        if (value)
            FlagManager.Instance.SetFlag(flagName);
        else
            FlagManager.Instance.RemoveFlag(flagName);
    }
}
