using System;
using UnityEngine;

public class EventManager
{
    private readonly EventDatabase _eventDatabase;

    private readonly GameManager _gameManager;
    private readonly FlagManager _flagManager;

    public EventSaveData EventSaveData;

    public event Action<EventStep> OnStepStarted;
    public event Action OnEventEnded;

    public EventManager(EventDatabase eventDatabase, GameManager gameManager, FlagManager flagManager)
    {
        _eventDatabase = eventDatabase;
        _gameManager = gameManager;
        _flagManager = flagManager;
    }

    public void OnEnterEvent()
    {
        StartEvent(_eventDatabase.GetRandomEvent(_gameManager, _flagManager));
    }

    private void StartEvent(MultiStepEvent evt)
    {
        EventSaveData.eventId = evt.EventId;
        EventSaveData.stepId = "start";
        EventSaveData.isCompleted = false;
        StartStep("start");
    }

    private void StartStep(string stepId)
    {
        EventSaveData.stepId = stepId;
        EventSaveData.isCompleted = false;

        // ロジックのみ：UIは操作しない
        var step = _eventDatabase.GetEvent(EventSaveData.eventId).GetStep(stepId);
        OnStepStarted?.Invoke(step);

        _gameManager.SaveEvent(EventSaveData);
    }

    public void SelectOption(EventOption option)
    {
        if (option.EndsEvent)
        {
            EventSaveData.isCompleted = true;
            OnEventEnded?.Invoke();
        }
        else
        {
            var nextId = ProcessOption(option);
            StartStep(nextId);
        }

        _gameManager.SaveEvent(EventSaveData);
    }

    private string ProcessOption(EventOption option)
    {
        if (!string.IsNullOrEmpty(option.FlagToSet))
            _flagManager.SetFlag(option.FlagToSet);
        if (!string.IsNullOrEmpty(option.FlagToRemove))
            _flagManager.RemoveFlag(option.FlagToRemove);

        string nextId = option.DefaultNextStepId;
        if (option.Condition != null && option.Condition.IsMet(_gameManager, _flagManager))
            nextId = option.ConditionalNextStepId;
        return nextId;
    }


    public void LoadData(EventSaveData data)
    {
        if (data.isCompleted) return;
        EventSaveData = data;
        StartStep(data.stepId);
    }

    private EventSaveData SaveEvent()
    {
        return EventSaveData;
    }

}