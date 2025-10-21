using System;
using UnityEngine;

public class EventRunner
{
    public event Action<EventStep> OnStepChanged;
    public event Action OnEventEnded;

    private MultiStepEvent currentEvent;
    private EventStep currentStep;

    public event Action<EventOption> OnOptionSelected;

    public void StartEvent(MultiStepEvent evt)
    {
        currentEvent = evt;
        currentStep = evt.Steps[0];
        OnStepChanged?.Invoke(currentStep);
    }

    public void SelectOption(EventOption option)
    {
        ProcessOption(option);

        if (option.EndsEvent)
        {
            OnOptionSelected?.Invoke(option);
        }
        else
        {
            StartStep(option.DefaultNextStepId);
        }
    }

    private void ProcessOption(EventOption option)
    {
        // --- フラグ処理 ---
        if (!string.IsNullOrEmpty(option.FlagToSet))
            FlagManager.Instance.SetFlag(option.FlagToSet);
        if (!string.IsNullOrEmpty(option.FlagToRemove))
            FlagManager.Instance.RemoveFlag(option.FlagToRemove);

        // --- 条件判定 ---
        string nextId = option.DefaultNextStepId;
        if (option.Condition != null && option.Condition.IsMet())
            nextId = option.ConditionalNextStepId;
    }

    public void StartStep(string stepId)
    {
        currentStep = currentEvent.GetStep(stepId);
        OnStepChanged?.Invoke(currentStep);
    }
}
