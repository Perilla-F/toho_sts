using System.Collections.Generic;

[System.Serializable]
public class EnemyActionPatternData
{
    public List<SerializableBattleAction> Actions;
    public SerializableEnemyCondition Condition;
}