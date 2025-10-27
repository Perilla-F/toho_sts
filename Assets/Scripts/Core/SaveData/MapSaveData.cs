using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapSaveData
{
    public Dictionary<Vector2Int, MapCellState> mapData;
    public int cellX;
    public int cellY;

    public MapSaveData(Dictionary<Vector2Int, MapCellState> mapData, int cellX, int cellY)
    {
        this.mapData = mapData;
        this.cellX = cellX;
        this.cellY = cellY;
    }
}