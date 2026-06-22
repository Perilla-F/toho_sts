using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class EnemyActionEvent : BattleEvent
{
    public EnemyAction action;

    public EnemyActionEvent(IHeroUnit hero, IEnemyUnit self, EnemyAction action, int scheduledTime, int priority)
        : base(scheduledTime, priority)
    {
        EnemyId = self.EnemyID;
        Enemy = self;
        this.action = action;
        Type = EventType.Enemy;
        ActionName = action.actionName;
        action.Self = self;
    }

    public override async UniTask Execute(IBattleContext context)
    {
        await action.Execute(context);
    }
}