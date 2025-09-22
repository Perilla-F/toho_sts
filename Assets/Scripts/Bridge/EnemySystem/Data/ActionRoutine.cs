using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/ActionRoutine")]
public class ActionRoutine : ScriptableObject
{
    public ActionPattern[] patterns;


    public EnemyAction[] GetNextAction(int turnCounter)
    {
        var pattern = patterns[turnCounter % patterns.Length];

        // 必要に応じてダメージなどを上書き
        foreach (var entry in pattern.entries)
        {
            foreach (var e in entry.actions)
            {
                foreach (var effect in e.action.effects)
                {
                    if (effect is DamageEffect dmg && e.damageOverride > 0)
                    {
                        dmg.amount = e.damageOverride;
                    }
                    else if (effect is EnemyStatusEffect dbf && e.amountOverride > 0)
                    {
                        dbf.amount = e.amountOverride;
                    }
                }
            }
        }
        return pattern.GetEnemyActions();
    }


}
