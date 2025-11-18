using System;
using UnityEngine;

public class EventManager
{
    private readonly EventDatabase _eventDatabase;

    private GameManager _gameManager;
    private readonly FlagManager _flagManager;
    private GameContext _context;

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
        _context.SetEventId(evt.EventId);
        _context.SetEventStepId("start");
        _context.SetEventConpleted(false);
        StartStep("start");
    }

    private void StartStep(string stepId)
    {
        _context.SetEventStepId(stepId);
        _context.SetEventConpleted(false);

        // ロジックのみ：UIは操作しない
        var step = _eventDatabase.GetEvent(_context.CurrentEventId).GetStep(stepId);
        OnStepStarted?.Invoke(step);

        _gameManager.RequestSave();
    }

    public void SelectOption(EventOption option)
    {
        if (option.EndsEvent)
        {
            _context.SetEventConpleted(true);
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
            _context.CurrentEventId,
            _context.CurrentStepId,
            _context.CurrentEventCompleted
        );
    }

    public void RestoreFrom(EventSaveData data)
    {
        StartStep(data.stepId);
    }

}