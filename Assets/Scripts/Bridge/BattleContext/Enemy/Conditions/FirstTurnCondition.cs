using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/FirstTurn")]
public class FirstTurnCondition : Condition
{
    public override bool Check(IBattleContext context, IEnemyUnit enemy)
    {
        return context.Turn == 1;
    }
}