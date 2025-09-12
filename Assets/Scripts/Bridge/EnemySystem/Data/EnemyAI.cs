using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/AI")]
public class EnemyAI : ScriptableObject
{
    public List<ConditionPattern> ConditionPatterns;

    public List<WeightedAction> GetActions(IBattleContext context, IBattleUnit self)
    {
        foreach (var data in ConditionPatterns)
        {
            if (data.condition.IsSatisfied(context, self))
            {
                return data.actions;
            }
        }
        return new List<WeightedAction>(); //fallback;\
    }

    // public EnemyTurnActions GetActions(ConditionContext context)
    // {
    //     foreach (var data in conditionDatas)
    //     {
    //         if (data.IsConditionMet(context))
    //         {
    //             return data.GetRandomPattern();
    //         }
    //     }
    //     return new EnemyTurnActions(); // fallback
    // }
}
