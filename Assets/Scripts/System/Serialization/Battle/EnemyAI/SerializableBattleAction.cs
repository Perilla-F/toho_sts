[System.Serializable]
public class SerializableBattleAction
{
    public string actionType; // 例: "Attack", "Defend", etc
    public float delay;
    public string parametersJson; // 必要なら追加情報を文字列で保持
}