using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/Conditional")]
public class ConditionalResult : EventResult
{
    public EventCondition condition;

    [Header("条件が TRUE の場合")]
    public EventResult trueResult;

    [Header("条件が FALSE の場合")]
    public EventResult falseResult;

    public override void Apply(IGameContext context, IFlagManager flags, EventOption option)
    {
        if (condition != null && condition.IsMet(context, flags))
        {
            if (trueResult != null)
                trueResult.Apply(context, flags, option);
        }
        else
        {
            if (falseResult != null)
                falseResult.Apply(context, flags, option);
        }
    }
}
