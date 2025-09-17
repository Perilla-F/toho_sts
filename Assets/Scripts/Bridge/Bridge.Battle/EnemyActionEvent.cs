public class EnemyActionEvent : BattleEvent
{
    private EnemyUnit enemy;
    private EnemyAction action;

    public EnemyActionEvent(EnemyUnit enemy, EnemyAction action, int scheduledTime, int priority)
        : base(scheduledTime, priority)
    {
        this.enemy = enemy;
        this.action = action;
    }

    public override void Execute(IBattleContext context)
    {
        action.Execute(context, enemy);
    }
}