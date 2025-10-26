using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/Composite")]
public class CompositeResult : EventResult
{
    public List<EventResult> results = new();

    public override void Apply(IGameManager game, IFlagManager flagManager)
    {
        foreach (var r in results)
            r?.Apply(game, flagManager);
    }
}
