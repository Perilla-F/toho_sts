using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/FirstTurn")]
public class FirstTurnCondition : Condition
{
    public override bool Check(BattleContext context, EnemyUnit enemy)
    {
        return context.Turn == 1;
    }
}