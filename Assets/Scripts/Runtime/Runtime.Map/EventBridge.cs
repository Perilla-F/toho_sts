using UnityEngine;

public class EventBridge : MonoBehaviour
{
    private SaveManager saveManager;

    private EventUIManager uiManager;
    public EventRunner runner { get; private set; }

    [Header("Event Data")]
    private EventDatabase EventDatabase;


    private void Awake()
    {
        saveManager = ServiceLocator.Get<SaveManager>();

        runner.OnStartStep += HandleStartStep;
        uiManager.OnOptionSelected += HandleOptionSelected;
    }

    private void HandleStartStep(MapSaveData mapSaveData)
    {
        saveManager.SaveMap(mapSaveData);
    }

    private void HandleOptionSelected(EventOption option)
    {
        runner.SelectOption(option);
    }

    /// <summary>
    /// イベント再開
    /// </summary>
    public void TryResumeLastEvent(SaveData saveData)
    {
        if (saveData.Map.lastEventData != null && !saveData.Map.lastEventData.isCompleted)
        {
            var evt = EventDatabase.GetEvent(saveData.Map.lastEventData.eventId) as MultiStepEvent;
            runner.StartStep(saveData.Map.lastEventData.stepId);
        }
    }

    private void OnDestroy()
    {
        if (runner == null) return;

        runner.OnStartStep -= HandleStartStep;
    }
}