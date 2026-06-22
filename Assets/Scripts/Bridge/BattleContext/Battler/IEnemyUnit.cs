using UnityEngine;

public interface IEnemyUnit : IBattleUnit
{
    public EnemyType EnemyType { get; }
    public ConditionType currentCondition { get; }
    public ConditionType lastCondition { get; }
    public int turnCounter { get; }
    public Sprite EventIcon { get; }
    public int EnemyID { get; }
    public abstract EnemyAction[] PlanTurn(IBattleContext context);

}