using System;
using System.Collections.Generic;

public static class EnemyAIFactory
{
    public static EnemyAI Build(EnemyAIData data)
    {
        var enemyAI = new EnemyAI();

        foreach (var patternData in data.Patterns)
        {
            IEnemyCondition condition = CreateCondition(patternData.ConditionType, patternData.ConditionValue, patternData.Status);
            List<BattleAction> actions = new();

            foreach (var actionData in patternData.Actions)
            {
                BattleAction action = new BattleAction(actionData);
                actions.Add(action);
            }

            var pattern = new EnemyActionPattern
            {
                Condition = condition,
                Actions = actions
            };

            enemyAI.Patterns.Add(pattern);
        }

        return enemyAI;
    }

    private static IEnemyCondition CreateCondition(EnemyConditionType type, int value, string status)
    {
        return type switch
        {
            EnemyConditionType.Always => new AlwaysTrueCondition(),
            EnemyConditionType.Turn => new TurnEqualsCondition(value),
            EnemyConditionType.HPBelow => new HpBelowCondition(value),
            EnemyConditionType.PlayerHasStatus => new PlayerHasStatusCondition(status),
            _ => throw new ArgumentException($"Unknown condition: {type}")
        };
    }
}
