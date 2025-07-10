using System.Collections.Generic;


[System.Serializable]
public class EnemyAI
{
    public List<EnemyActionPattern> patterns = new();
    private int turnCounter = 0;

    public List<BattleAction> GetActions(EnemyUnit self, int turn, ConditionContext context)
    {
        foreach (var pattern in patterns)
        {
            if (pattern.Condition == null || pattern.Condition.IsMet(self, turnCounter, context))
            {
                return pattern.Actions;
            }
        }
        return new List<BattleAction>(); // fallback
    }

}
