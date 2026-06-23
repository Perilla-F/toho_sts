using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/FirstTurn")]
public class FirstTurnCondition : Condition
{
    public override bool Check(IReadOnlyBattleContext context, IReadOnlyEnemyUnit enemy)
    {
        return context.Turn == 1;
    }
}