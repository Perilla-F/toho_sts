[System.Serializable]
public class LastEventData
{
    public string eventId;
    public string selectedOptionId;  // 選択肢（例: "A", "B", "C"）
    public bool isCompleted;         // イベント完了済みかどうか
}