using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Event/MultiStepEvent")]
public class MultiStepEvent : ScriptableObject
{
    public string EventId;
    public string EventName;
    public int Weight;
    [TextArea(2, 4)] public string Description;

    public List<EventStep> Steps; // 全ステップ一覧

    public EventStep GetStep(string stepId)
    {
        return Steps.Find(s => s.StepId == stepId);
    }
}