using System.Collections.Generic;
using UnityEngine;

namespace Serialization.EnemyAI
{
    public static class EnemyAISerializer
    {
        public static EnemyAISerializableData ToSerializableData(EnemyAIEditorAsset asset)
        {
            var data = new EnemyAISerializableData
            {
                enemyId = asset.enemyId,
                patterns = new List<EnemyActionPatternData>()
            };

            foreach (var pattern in asset.actionPatterns)
            {
                var patternData = new EnemyActionPatternData
                {
                    actions = new List<SerializableBattleAction>(),
                    condition = ConvertCondition(pattern.condition)
                };

                foreach (var action in pattern.actions)
                {
                    patternData.actions.Add(new SerializableBattleAction
                    {
                        actionType = action.actionType.ToString(),
                        delay = action.delay,
                        targetId = action.targetId,
                        parametersJson = JsonUtility.ToJson(action.parametersJson) // 追加情報
                    });
                }

                data.patterns.Add(patternData);
            }

            return data;
        }

        private static SerializableEnemyCondition ConvertCondition(IEnemyCondition condition)
        {
            if (condition is HpBelowCondition hpCond)
            {
                return new SerializableEnemyCondition
                {
                    conditionType = "HPBelow",
                    value = hpCond.threshold.ToString()
                };
            }

            if (condition is TurnEqualsCondition turnCond)
            {
                return new SerializableEnemyCondition
                {
                    conditionType = "TurnEquals",
                    value = turnCond.turnNumber.ToString()
                };
            }

            // デフォルト
            return new SerializableEnemyCondition
            {
                conditionType = "Always",
                value = ""
            };
        }
    }
}
