using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/Composite")]
public class CompositeResult : EventResult
{
    public List<EventResult> results = new();

    public override void Apply(GameContext context, IFlagManager flagManager)
    {
        foreach (var r in results)
            r?.Apply(context, flagManager);
    }
}
