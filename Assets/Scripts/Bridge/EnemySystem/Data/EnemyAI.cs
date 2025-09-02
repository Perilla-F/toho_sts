using System.Collections.Generic;


[System.Serializable]
public class EnemyAI
{
    public List<EnemyActionPattern> Patterns = new();
    private int _turnCounter = 0;

    public List<BattleAction> GetActions(EnemyUnit self, int turn, ConditionContext context)
    {
        foreach (var pattern in Patterns)
        {
            if (pattern.Condition == null || pattern.Condition.IsMet(self, _turnCounter, context))
            {
                return pattern.Actions;
            }
        }
        return new List<BattleAction>(); // fallback
    }

}
