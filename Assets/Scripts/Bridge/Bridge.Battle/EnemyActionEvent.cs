public class EnemyActionEvent : BattleEvent
{
    private IBattleUnit enemy;
    private EnemyAction action;

    public EnemyActionEvent(IBattleUnit enemy, EnemyAction action, int scheduledTime, int priority)
        : base(scheduledTime, priority)
    {
        this.enemy = enemy;
        this.action = action;
    }

    public override void Execute(IBattleContext context)
    {
        action.Perform(context);
    }
}