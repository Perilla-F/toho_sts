using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Event/EventDatabase")]
public class EventDatabase : ScriptableObject
{
    [SerializeField] private List<MultiStepEvent> events = new();
    private Dictionary<string, MultiStepEvent> dict;

    public static EventDatabase Instance;

    private void OnEnable()
    {
        Instance = this;
        dict = new Dictionary<string, MultiStepEvent>();
        foreach (var evt in events)
        {
            dict[evt.EventId] = evt;
        }
    }

    public MultiStepEvent GetEvent(string id)
    {
        dict ??= new Dictionary<string, MultiStepEvent>();
        if (dict.TryGetValue(id, out var evt))
            return evt;

        Debug.LogWarning($"Event not found: {id}");
        return null;
    }

    public MultiStepEvent GetRandomEvent(GameContext context, IFlagManager flagManager)
    {
        if (events == null || events.Count == 0)
        {
            Debug.LogWarning("No events registered in EventDatabase.");
            return null;
        }

        // 1. 発生済みイベントや条件未達イベントを除外
        List<MultiStepEvent> available = new();

        foreach (var e in events)
        {
            // 発生済みフラグをチェック
            if (flagManager.HasFlag($"Event_{e.EventId}_Done"))
                continue;

            // 最初のステップ条件を満たしているか？
            var firstStep = e.Steps.Count > 0 ? e.Steps[0] : null;
            if (firstStep == null)
                continue;

            // 最初のステップに条件付き選択肢がある場合、最低1つは有効か？
            bool valid = false;
            foreach (var option in firstStep.Options)
            {
                if (option.Condition == null || option.Condition.IsMet(context, flagManager))
                {
                    valid = true;
                    break;
                }
            }

            if (valid)
                available.Add(e);
        }

        // 2. 対象がなければ全イベントから再抽選
        if (available.Count == 0)
            available.AddRange(events);

        // 3. ランダム選出
        int index = Random.Range(0, available.Count);
        return available[index];
    }

}
