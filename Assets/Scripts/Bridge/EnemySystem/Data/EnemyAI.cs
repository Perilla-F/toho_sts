using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyAI")]
public class EnemyAI : ScriptableObject
{
    public string EnemyId;
    public List<EnemyConditionData> conditionDatas;

    public EnemyTurnActions GetActions(EnemyUnit self, int turn, ConditionContext context)
    {
        foreach (var data in conditionDatas)
        {
            if (data.IsConditionMet(self.CurrentHP / self.MaxHP, turn, ""))
            {
                return data.Actions[turn % data.Actions.Count];
            }
        }
        return new EnemyTurnActions(); // fallback
    }

}
