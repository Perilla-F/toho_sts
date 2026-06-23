[System.Serializable]
public class EventSaveData
{
    public string eventId;
    public string stepId;
    public bool isCompleted;

    public EventSaveData(string eventId, string stepId, bool isCompleted)
    {
        this.eventId = eventId;
        this.stepId = stepId;
        this.isCompleted = isCompleted;
    }
}