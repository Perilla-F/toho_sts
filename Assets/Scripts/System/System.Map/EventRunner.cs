using System;
using UnityEngine;

public class EventRunner
{
    private IGameManager game;
    private IFlagManager flagManager;

    public event Action<EventStep> OnStepChanged;
    public event Action OnEventEnded;
    private IEventView eventView;

    public event Action<EventStep> OnStartStep;

    private MultiStepEvent currentEvent;
    private EventStep currentStep;

    public event Action<EventOption> OnOptionSelected;

    public EventRunner(IGameManager game, IFlagManager flagManager)
    {
        this.game = game;
        this.flagManager = flagManager;
    }

    public EventSaveData EventSaveData { get; set; }

    public EventRunner(IEventView eventView)
    {
        this.eventView = eventView;
    }

    /// <summary>
    /// イベント開始
    /// </summary>
    /// <param name="evt"></param>
    public void StartEvent(MultiStepEvent evt)
    {
        currentEvent = evt;
        EventSaveData = new EventSaveData
        (
            evt.EventId,
            null,
            false
        );
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
            if (EventSaveData == null) return;
            EventSaveData.isCompleted = true;
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
            flagManager.SetFlag(option.FlagToSet);
        if (!string.IsNullOrEmpty(option.FlagToRemove))
            flagManager.RemoveFlag(option.FlagToRemove);

        // --- 条件判定 ---
        string nextId = option.DefaultNextStepId;
        if (option.Condition != null && option.Condition.IsMet(game, flagManager))
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
        EventSaveData = new EventSaveData
        (
            currentEvent.EventId,
            stepId,
            false
        );
        OnStartStep?.Invoke(currentStep);
        eventView.ShowStep(currentStep);
    }

}
