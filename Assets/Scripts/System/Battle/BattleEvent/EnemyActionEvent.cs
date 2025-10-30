using UnityEngine.UI;

public class EnemyActionEvent : BattleEvent, IEnemyActionEvent
{
    public IEnemyUnit Enemy { get; }
    private EnemyAction action;
    public IEnemyManager ReferenceEnemy { get; }

    public EnemyActionEvent(EnemyUnit enemy, EnemyAction action, int scheduledTime, int priority)
        : base(scheduledTime, priority)
    {
        this.Enemy = enemy;
        this.action = action;
    }

    public override void Execute(BattleContext context)
    {
        action.Execute(context, Enemy);
    }
}