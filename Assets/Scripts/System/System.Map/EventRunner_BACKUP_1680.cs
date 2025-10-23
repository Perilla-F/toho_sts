using System;
using UnityEngine;

public class EventRunner
{
<<<<<<< HEAD
    private IEventView eventView;

    public event Action<EventStep> OnStartStep;
=======
    private GameContext context;
    private IFlagManager flagManager;

    public event Action<EventStep> OnStepChanged;
    public event Action OnEventEnded;
>>>>>>> origin/battle-system-laptop

    private MultiStepEvent currentEvent;
    private EventStep currentStep;

    public event Action<EventOption> OnOptionSelected;

<<<<<<< HEAD
    public LastEventData LastEventData { get; set; }

    public EventRunner(IEventView eventView)
    {
        this.eventView = eventView;
    }

    /// <summary>
    /// イベント開始
    /// </summary>
    /// <param name="evt"></param>
=======
    public EventRunner(GameContext context, IFlagManager flagManager)
    {
        this.context = context;
        this.flagManager = flagManager;
    }

>>>>>>> origin/battle-system-laptop
    public void StartEvent(MultiStepEvent evt)
    {
        currentEvent = evt;
        LastEventData = new LastEventData
        {
            eventId = evt.EventId,
            stepId = null,
            isCompleted = false,
        };
        currentStep = evt.GetStep("start");
        eventView.ShowStep(currentStep);
    }

    /// <summary>
    /// 選択肢を押したら
    /// </summary>
    /// <param name="option"></param>
    public void SelectOption(EventOption option)
    {
        var nextId = ProcessOption(option);

        if (option.EndsEvent)
        {
            OnOptionSelected?.Invoke(option);
            eventView.Hide();
            if (LastEventData == null) return;
            LastEventData.isCompleted = true;
        }
        else
        {
            StartStep(nextId);
        }
    }

    /// <summary>
    /// 選択肢の分岐
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    private string ProcessOption(EventOption option)
    {
        // --- フラグ処理 ---
        if (!string.IsNullOrEmpty(option.FlagToSet))
            FlagManager.Instance.SetFlag(option.FlagToSet);
            flagManager.SetFlag(option.FlagToSet);
        if (!string.IsNullOrEmpty(option.FlagToRemove))
            FlagManager.Instance.RemoveFlag(option.FlagToRemove);
            flagManager.RemoveFlag(option.FlagToRemove);

        // --- 条件判定 ---
        string nextId = option.DefaultNextStepId;
        if (option.Condition != null && option.Condition.IsMet())
        if (option.Condition != null && option.Condition.IsMet(context, flagManager))
            nextId = option.ConditionalNextStepId;
        return nextId;
    }

    /// <summary>
    /// ステップ開始
    /// </summary>
    /// <param name="stepId"></param>
    public void StartStep(string stepId)
    {
        currentStep = currentEvent.GetStep(stepId);
        LastEventData = new LastEventData
        {
            eventId = currentEvent.EventId,
            stepId = stepId,
            isCompleted = false,
        };
        OnStartStep?.Invoke(currentStep);
        eventView.ShowStep(currentStep);
    }

}
