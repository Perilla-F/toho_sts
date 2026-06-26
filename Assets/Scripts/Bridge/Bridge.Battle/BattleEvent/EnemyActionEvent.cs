using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyActionEvent : BattleEvent
{
    public EnemyAction action;
    public int EnemyId { get; set; }

    public EnemyActionEvent(Sprite eventIcon, int id, EnemyAction action, int scheduledTime, int priority)
        : base(eventIcon, scheduledTime, priority)
    {
        EnemyId = id;
        this.action = action;
        Type = EventType.Enemy;
        ActionName = action.actionName;
    }

    public override async UniTask Execute(IBattleContext context)
    {
        await action.Execute(EnemyId, context);
    }
}