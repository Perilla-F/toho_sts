using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/Sequence")]
public class SequenceResult : EventResult
{
    public EventResult[] results;

    public override void Apply(IGameContext context, IFlagManager flags, EventOption option)
    {
        foreach (var r in results)
        {
            if (r != null)
                r.Apply(context, flags, option);
        }
    }
}