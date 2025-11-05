using UnityEngine.UI;
using System.Threading.Tasks;

public class EnemyActionEvent : BattleEvent, IEnemyActionEvent
{
    private EnemyAction action;
    public IEnemyManager ReferenceEnemy { get; }

    public EnemyActionEvent(EnemyUnit enemy, EnemyAction action, int scheduledTime, int priority)
        : base(scheduledTime, priority)
    {
        EnemyId = enemy.EnemyID;
        Enemy = enemy;
        this.action = action;
    }

    public override async Task Execute(BattleContext context)
    {
        action.Execute(context, Enemy);
    }
}