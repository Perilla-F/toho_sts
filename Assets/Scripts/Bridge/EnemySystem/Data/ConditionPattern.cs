using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionPattern
{
    public EnemyCondition condition; // HP低下, ターン数など
    public List<WeightedAction> actions; // 抽選
}