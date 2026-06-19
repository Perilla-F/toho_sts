using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class EnemyActionEvent : BattleEvent, IEnemyActionEvent
{
    public EnemyAction action;

    public EnemyActionEvent(IEnemyUnit enemy, EnemyAction action, int scheduledTime, int priority)
        : base(scheduledTime, priority)
    {
        EnemyId = enemy.EnemyID;
        Enemy = enemy;
        this.action = action;
        Type = EventType.Enemy;
        ActionName = action.actionName;
    }

    public override async UniTask Execute(IBattleContext context)
    {
        await action.Execute(context, Enemy);
    }
}