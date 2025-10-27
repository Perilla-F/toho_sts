using UnityEngine;

public class EventBridge : MonoBehaviour
{
    public static EventBridge Instance { get; private set; }

    [SerializeField] private EventUIManager uiManager;
    public EventRunner runner { get; private set; }
    private SaveManager saveManager;

    [Header("Event Data")]
    [SerializeField] private EventDatabase EventDatabase;


    private void Awake()
    {
        Instance = this;
        runner.OnStartStep += HandleStartStep;
        uiManager.OnOptionSelected += HandleOptionSelected;
    }

    private void HandleStartStep(EventStep step)
    {
        //        saveManager.SaveGame();
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