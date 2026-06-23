using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/Always")]
public class AlwaysCondition : Condition
{
    public override bool Check(IReadOnlyBattleContext context, IReadOnlyEnemyUnit enemy)
    {
        return true;
    }
}