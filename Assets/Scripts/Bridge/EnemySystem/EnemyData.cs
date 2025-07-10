using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject, IBattlerBaseData
{
    public string enemyId;
    public string BattlerName { get; }
    public int MaxHP { get; }
    public RuntimeAnimatorController AnimatorController { get; }
    public int rewardGold;
}