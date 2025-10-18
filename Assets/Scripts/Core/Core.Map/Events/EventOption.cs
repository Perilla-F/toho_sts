[System.Serializable]
public class EventOption
{
    public string Id;                    // 選択肢ID（例："A", "B"）
    public string Text;                  // ボタンに表示する文言
    public string NextStepId;            // 選択後に進む次のステップ
    public bool EndsEvent;               // この選択でイベントを終了するか
}