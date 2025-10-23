using UnityEngine.UI;

public class EnemyActionEvent : BattleEvent
{
    public EnemyUnit Enemy;
    private EnemyAction action;
    public EnemyManager ReferenceEnemy;

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