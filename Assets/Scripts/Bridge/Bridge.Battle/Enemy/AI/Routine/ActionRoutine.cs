using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/ActionRoutine")]
public class ActionRoutine : ScriptableObject
{
    public ActionPattern[] patterns;

    public EnemyAction[] GetNextAction(int turnCounter)
    {
        var pattern = patterns[turnCounter % patterns.Length];

        return pattern.GetEnemyActions();
    }

}
