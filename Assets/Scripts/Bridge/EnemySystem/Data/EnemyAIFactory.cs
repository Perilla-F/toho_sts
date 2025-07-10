using System;
using System.Collections.Generic;

public static class EnemyAIFactory
{
    public static EnemyAI Build(EnemyAIData data, EnemyUnit self, HeroUnit Hero, EnemyManager enemies)
    {
        var enemyAI = new EnemyAI();

        foreach (var patternData in data.patterns)
        {
            IEnemyCondition condition = CreateCondition(patternData.conditionType, patternData.conditionValue, patternData.status);
            List<BattleAction> actions = new();


            foreach (var actionData in patternData.actions)
            {
                BattleAction action = CreateAction(actionData.actionType, actionData.value, actionData.target, self, Hero, enemies);
                actions.Add(action);
            }

            var pattern = new EnemyActionPattern
            {
                Condition = condition,
                Actions = actions
            };

            enemyAI.patterns.Add(pattern);
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

    private static BattleAction CreateAction(EnemyActionType type, int value, EnemyActionTarget targets, EnemyUnit self, HeroUnit hero, EnemyManager enemies)
    {
        return type switch
        {
            EnemyActionType.Attack => new EnemyAttackAction(self, value, targets, hero, enemies),
            EnemyActionType.Defend => new EnemyDefendAction(self, value, targets, hero, enemies),
            EnemyActionType.AttackBuff => new EnemyAttackBuffAction(self, value, targets, hero, enemies),
            _ => throw new ArgumentException($"Unknown action: {type}")
        };
    }
}
