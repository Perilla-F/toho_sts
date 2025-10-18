using UnityEngine;

[CreateAssetMenu(menuName = "Events/Condition")]
public class EventCondition : ScriptableObject
{
    public EventConditionType type = EventConditionType.None;
    public int intValue;
    public string stringValue;

    public bool IsMet()
    {
        switch (type)
        {
            case EventConditionType.HPAtLeast:
                return GameManager.Instance.HeroBattler.HPResource.GetHP() >= intValue;
            case EventConditionType.HPAtMost:
                return GameManager.Instance.HeroBattler.HPResource.GetHP() <= intValue;
            case EventConditionType.FlagSet:
                return FlagManager.Instance.HasFlag(stringValue);
            default:
                return true;
        }
    }
}