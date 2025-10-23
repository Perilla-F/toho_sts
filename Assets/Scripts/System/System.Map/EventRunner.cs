using System;
using UnityEngine;

public class EventRunner
{
    private GameContext context;
    private IFlagManager flagManager;

    public event Action<EventStep> OnStepChanged;
    public event Action OnEventEnded;

    private MultiStepEvent currentEvent;
    private EventStep currentStep;

    public event Action<EventOption> OnOptionSelected;

    public EventRunner(GameContext context, IFlagManager flagManager)
    {
        this.context = context;
        this.flagManager = flagManager;
    }

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
            flagManager.SetFlag(option.FlagToSet);
        if (!string.IsNullOrEmpty(option.FlagToRemove))
            flagManager.RemoveFlag(option.FlagToRemove);

        // --- 条件判定 ---
        string nextId = option.DefaultNextStepId;
        if (option.Condition != null && option.Condition.IsMet(context, flagManager))
            nextId = option.ConditionalNextStepId;
    }

    public void StartStep(string stepId)
    {
        currentStep = currentEvent.GetStep(stepId);
        OnStepChanged?.Invoke(currentStep);
    }
}
