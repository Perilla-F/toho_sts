using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Encounter", menuName = "GameData/Encounter")]
public class EncounterData : ScriptableObject
{
    public string EncounterID;
    public EncounterType Type;

    [Header("敵データ")]
    public List<EnemyData> Enemies;

    [Header("UI座標（Canvas上のローカル座標）")]
    public List<Vector2> UIPositions;
}
