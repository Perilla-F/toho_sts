using UnityEngine;

[CreateAssetMenu(menuName = "Events/Condition")]
public class EventCondition : ScriptableObject
{
    public EventConditionType type = EventConditionType.None;
    public int intValue;
    public string stringValue;

    public bool IsMet(IGameContext context, IFlagManager flagManager)
    {
        switch (type)
        {
            case EventConditionType.HPAtLeast:
                return context.Hero.HPResource.GetHP() >= intValue;
            case EventConditionType.HPAtMost:
                return context.Hero.HPResource.GetHP() <= intValue;
            case EventConditionType.FlagSet:
                return flagManager.HasFlag(stringValue);
            default:
                return true;
        }
    }
}