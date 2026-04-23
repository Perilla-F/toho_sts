using UnityEngine.UI;
using System.Threading.Tasks;

public class EnemyActionEvent : BattleEvent, IEnemyActionEvent
{
    private EnemyAction action;

    public EnemyActionEvent(IEnemyUnit enemy, EnemyAction action, int scheduledTime, int priority)
        : base(scheduledTime, priority)
    {
        EnemyId = enemy.EnemyID;
        Enemy = enemy;
        this.action = action;
        Type = EventType.Enemy;
        ActionName = action.actionName;
    }

    public override async Task Execute(IBattleContext context)
    {
        action.Execute(context, Enemy);
    }
}