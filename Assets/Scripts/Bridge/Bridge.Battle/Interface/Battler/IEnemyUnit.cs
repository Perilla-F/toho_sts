using UnityEngine;

public interface IEnemyUnit : IBattleUnit, IReadOnlyEnemyUnit
{
}

public interface IReadOnlyEnemyUnit : IReadOnlyBattleUnit
{
    public int EnemyID { get; }
    public EnemyType EnemyType { get; }
    public ConditionType currentCondition { get; }
    public ConditionType lastCondition { get; }
    public int turnCounter { get; }
    public Sprite EventIcon { get; }

    /// <summary>
    /// AIから行動をターン中の行動をリストで引く
    /// </summary>
    /// <param name="turn"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public abstract EnemyAction[] PlanTurn(IBattleContext context);
}