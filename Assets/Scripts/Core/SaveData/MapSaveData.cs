using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapSaveData
{
    public Dictionary<Vector2Int, MapCellState> mapData;
    public int cellX;
    public int cellY;
    public int stageIndex;

    public MapSaveData(Dictionary<Vector2Int, MapCellState> mapData, int cellX, int cellY, int stageIndex)
    {
        this.mapData = mapData;
        this.cellX = cellX;
        this.cellY = cellY;
        this.stageIndex = stageIndex;
    }
}