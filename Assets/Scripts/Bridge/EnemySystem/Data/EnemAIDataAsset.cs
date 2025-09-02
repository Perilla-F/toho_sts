using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyAI", menuName = "AI/EnemyAI")]
public class EnemyAIEditorAsset : ScriptableObject
{
    public string EnemyId;
    public List<EnemyPatternData> ActionPatterns = new();
}