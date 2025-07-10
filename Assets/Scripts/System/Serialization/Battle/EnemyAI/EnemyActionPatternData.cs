using System.Collections.Generic;

[System.Serializable]
public class EnemyActionPatternData
{
    public List<SerializableBattleAction> actions;
    public SerializableEnemyCondition condition;
}