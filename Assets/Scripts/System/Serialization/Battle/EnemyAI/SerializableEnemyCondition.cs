[System.Serializable]
public class SerializableEnemyCondition
{
    public string conditionType; // 例: "HPBelow", "TurnEquals"
    public string value; // 条件に応じた値（HPなら50、ターン数なら3など）
}