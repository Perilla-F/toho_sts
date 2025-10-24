using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapSaveData
{
    public Dictionary<Vector2Int, MapCellState> mapData;
    public int cellX;
    public int cellY;
    public EventSaveData lastEventData;

    public MapSaveData(Dictionary<Vector2Int, MapCellState> mapData, int cellX, int cellY, EventSaveData lastEventData)
    {
        this.mapData = mapData;
        this.cellX = cellX;
        this.cellY = cellY;
        this.lastEventData = lastEventData;
    }
}