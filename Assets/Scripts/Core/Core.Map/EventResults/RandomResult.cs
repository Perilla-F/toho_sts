using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "Events/Results/Random")]
public class RandomResult : EventResult
{
    [System.Serializable]
    public class RandomEntry
    {
        public EventResult result;  // 実際に発動する結果
        public int weight = 1;      // 確率重み
    }

    public RandomEntry[] entries;

    public override async UniTask Apply(IGameContext context, IFlagManager flags, EventOption option)
    {
        EventResult chosen = ChooseRandomResult();
        await chosen.Apply(context, flags, option); // 選ばれた結果を実行
    }

    private EventResult ChooseRandomResult()
    {
        int totalWeight = 0;
        foreach (var e in entries)
            totalWeight += e.weight;

        int rand = Random.Range(0, totalWeight);

        foreach (var e in entries)
        {
            if (rand < e.weight)
                return e.result;
            rand -= e.weight;
        }
        return entries[0].result; // fallback
    }
}
