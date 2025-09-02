[System.Serializable]
public class SerializableBattleAction
{
    public string ActionType; // 例: "Attack", "Defend", etc
    public float Delay;
    public string ParametersJson; // 必要なら追加情報を文字列で保持
}