[System.Serializable]
public class SerializableEnemyCondition
{
    public string ConditionType; // 例: "HPBelow", "TurnEquals"
    public string Value; // 条件に応じた値（HPなら50、ターン数なら3など）
}