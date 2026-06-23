using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class MapSaveData
{
    public List<CellPositionData> mapData;
    public int cellX;
    public int cellY;
    public int stageIndex;

    public MapSaveData(Dictionary<Vector2Int, MapCellState> mapData, int cellX, int cellY, int stageIndex)
    {
        this.mapData = new List<CellPositionData>();
        foreach (var data in mapData)
        {
            this.mapData.Add(new CellPositionData(data.Key, data.Value));
        }
        this.cellX = cellX;
        this.cellY = cellY;
        this.stageIndex = stageIndex;
    }
}