using System;
using UnityEngine;

public class EventManager
{
    private readonly EventDatabase _eventDatabase;

    private GameManager _gameManager;
    private readonly FlagManager _flagManager;
    private GameContext _context;

    public string CurrentEventId;
    public string CurrentStepId;
    public bool CurrentEventCompleted;

    public event Action<EventStep> OnStepStarted;
    public event Action OnEventEnded;

    public EventManager(EventDatabase eventDatabase, GameManager gameManager, FlagManager flagManager, GameContext context)
    {
        _eventDatabase = eventDatabase;
        _gameManager = gameManager;
        _flagManager = flagManager;
        _context = context;
    }

    public void Inject(GameManager gameManager, GameContext context)
    {
        _gameManager = gameManager;
        _context = context;
    }

    public void OnEnterEvent()
    {
        StartEvent(_eventDatabase.GetRandomEvent(_context, _flagManager));
    }

    private void StartEvent(MultiStepEvent evt)
    {
        CurrentEventId = evt.EventId;
        CurrentStepId = "start";
        CurrentEventCompleted = false;
        StartStep("start");
    }

    private void StartStep(string stepId)
    {
        CurrentStepId = stepId;
        CurrentEventCompleted = false;

        // ロジックのみ：UIは操作しない
        var step = _eventDatabase.GetEvent(CurrentEventId).GetStep(stepId);
        OnStepStarted?.Invoke(step);

        _gameManager.RequestSave();
    }

    public void SelectOption(EventOption option)
    {
        if (option.EndsEvent)
        {
            CurrentEventCompleted = true;
            OnEventEnded?.Invoke();
        }
        else
        {
            var nextId = ProcessOption(option);
            StartStep(nextId);
        }

        _gameManager.RequestSave();
    }

    private string ProcessOption(EventOption option)
    {
        if (!string.IsNullOrEmpty(option.FlagToSet))
            _flagManager.SetFlag(option.FlagToSet);
        if (!string.IsNullOrEmpty(option.FlagToRemove))
            _flagManager.RemoveFlag(option.FlagToRemove);

        string nextId = option.DefaultNextStepId;
        if (option.Condition != null && option.Condition.IsMet(_context, _flagManager))
            nextId = option.ConditionalNextStepId;
        return nextId;
    }

    public EventSaveData CreateSaveData()
    {
        return new EventSaveData(
            CurrentEventId,
            CurrentStepId,
            CurrentEventCompleted
        );
    }

    public void RestoreFrom(EventSaveData data)
    {
        if (data.isCompleted) return;
        CurrentEventId = data.eventId;
        CurrentStepId = data.stepId;
        CurrentEventCompleted = data.isCompleted;
        StartStep(data.stepId);
    }

}