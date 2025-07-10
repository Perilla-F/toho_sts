using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyAIData")]
public class EnemyAIData : ScriptableObject
{
    public string enemyId;
    public List<EnemyPatternData> patterns;
}
