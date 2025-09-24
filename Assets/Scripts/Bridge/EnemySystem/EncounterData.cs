using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Encounter", menuName = "GameData/Encounter")]
public class EncounterData : ScriptableObject
{
    public string encounterId;

    [Header("敵データ")]
    public List<EnemyData> enemies;

    [Header("UI座標（Canvas上のローカル座標）")]
    public List<Vector2> uiPositions;
}
