using System.Collections.Generic;

[System.Serializable]
public class EnemyPatternData
{
    public EnemyConditionType ConditionType;
    public int ConditionValue;
    public string Status;
    public List<EnemyActionData> Actions = new();
}