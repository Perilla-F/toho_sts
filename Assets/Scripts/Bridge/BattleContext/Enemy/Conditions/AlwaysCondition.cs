using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/Always")]
public class AlwaysCondition : Condition
{
    public override bool Check(IBattleContext context, IEnemyUnit enemy)
    {
        return true;
    }
}