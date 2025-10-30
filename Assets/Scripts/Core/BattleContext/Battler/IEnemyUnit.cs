using UnityEngine;

public abstract class IEnemyUnit : BattleUnit
{
    public EnemyType EnemyType;
    public ConditionType currentCondition = ConditionType.Turn;
    public ConditionType lastCondition;
    public int turnCounter = 0;
    public Sprite EventIcon;
    public int EnemyID;

}