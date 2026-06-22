using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Encounter", menuName = "GameData/Encounter")]
public class EncounterData : ScriptableObject
{
    [Header("エンカウンターグループID")]
    public string EncounterID;

    [Header("敵データ")]
    public List<EnemyData> Enemies;

    [Header("UI座標（Canvas上のローカル座標）")]
    public List<Vector2> UIPositions;

    [Header("UI座標（Canvas上の拡大）")]
    public List<int> ModelScale;
}
