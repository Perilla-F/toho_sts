using System.Collections.Generic;

[System.Serializable]
public class EnemyActionPattern
{
    public IEnemyCondition Condition;
    public List<BattleAction> Actions;
}