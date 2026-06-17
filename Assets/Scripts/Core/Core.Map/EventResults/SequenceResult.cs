using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "Events/Results/Sequence")]
public class SequenceResult : EventResult
{
    public EventResult[] results;

    public override async UniTask Apply(IGameContext context, IFlagManager flags, EventOption option)
    {
        foreach (var r in results)
        {
            if (r != null)
                await r.Apply(context, flags, option);
        }
    }
}