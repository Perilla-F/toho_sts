using System;
using UnityEngine;

class EventManager : MonoBehaviour
{
    [SerializeField] public EventDatabase EventDatabase;

    private GameManager gameManager;
    private SaveManager saveManager;
    private FlagManager flagManager;
    private PlayerManager playerManager;
    private EventUIManager uiManager;

    public EventSaveData EventSaveData;

    public event Action<EventOption> OnOptionSelected;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        gameManager = ServiceLocator.Get<GameManager>();
        saveManager = ServiceLocator.Get<SaveManager>();
        flagManager = ServiceLocator.Get<FlagManager>();

        uiManager.OnOptionSelected += SelectOption;
    }

    public void OnEnterEvent()
    {
        StartEvent(EventDatabase.GetRandomEvent(gameManager, flagManager));
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
        uiManager.ShowStep(EventDatabase.GetEvent(EventSaveData.eventId).GetStep(stepId));
        saveManager.SaveEvent(EventSaveData);
    }

    /// <summary>
    /// 選択肢を押したら
    /// </summary>
    /// <param name="option"></param>
    public void SelectOption(EventOption option)
    {
        if (option.EndsEvent)
        {
            OnOptionSelected?.Invoke(option);
            uiManager.Hide();
            EventSaveData.isCompleted = true;
        }
        else
        {
            var nextId = ProcessOption(option);
            StartStep(nextId);
        }
        saveManager.SaveEvent(EventSaveData);
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
        if (option.Condition != null && option.Condition.IsMet(gameManager, flagManager))
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

    public void StartBattle() { }

    public void StartEliteBattle() { }

    public void StartBossBattle() { }

    public void StartEvent() { }
    public void StartShop() { }

    public void StartRest() { }

    public void StartTreasure() { }

    public void StartGoal() { }

}