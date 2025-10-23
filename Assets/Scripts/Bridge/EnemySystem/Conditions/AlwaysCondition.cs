using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/Always")]
public class AlwaysCondition : Condition
{
    public override bool Check(BattleContext context, EnemyUnit enemy)
    {
        return true;
    }
}