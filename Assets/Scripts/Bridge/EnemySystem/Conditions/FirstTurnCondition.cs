using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/FirstTurn")]
public class FirstTurnCondition : Condition
{
    public override bool Check(IBattleContext context, EnemyUnit enemy)
    {
        return context.Turn == 1;
    }
}