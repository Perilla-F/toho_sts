using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/EnemyAIData")]
public class EnemyAIData : ScriptableObject
{
    public AIRule[] rules;

    public EnemyAction[] DecideActionPattern(IBattleContext context, EnemyUnit enemy, int turnCounter)
    {
        var ordered = rules.OrderBy(r => r.priority);
        foreach (var rule in ordered)
        {
            if (rule.condition.Check(context, enemy))
            {
                return rule.routine.GetNextAction(turnCounter);
            }
        }
        return null;
    }

}
