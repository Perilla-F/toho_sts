using System.Collections.Generic;

[System.Serializable]
public class EnemyPatternData
{
    public EnemyConditionType conditionType;
    public IEnemyCondition condition;
    public int conditionValue;
    public string status;
    public List<EnemyActionData> actions = new();
}