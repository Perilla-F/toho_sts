using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyAI", menuName = "AI/EnemyAI")]
public class EnemyAIEditorAsset : ScriptableObject
{
    public string enemyId;
    public List<EnemyPatternData> actionPatterns = new();
}