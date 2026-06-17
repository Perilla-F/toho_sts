using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "Events/Results/Conditional")]
public class ConditionalResult : EventResult
{
    public EventCondition condition;

    [Header("条件が TRUE の場合")]
    public EventResult trueResult;

    [Header("条件が FALSE の場合")]
    public EventResult falseResult;

    public override async UniTask Apply(IGameContext context, IFlagManager flags, EventOption option)
    {
        if (condition != null && condition.IsMet(context, flags))
        {
            if (trueResult != null)
                await trueResult.Apply(context, flags, option);
        }
        else
        {
            if (falseResult != null)
                await falseResult.Apply(context, flags, option);
        }
    }
}
