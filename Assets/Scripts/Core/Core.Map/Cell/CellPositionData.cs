using UnityEngine;

[System.Serializable]
public class CellPositionData
{
    public Vector2Int Position;
    public MapCellState MapCellState;

    public CellPositionData(Vector2Int pos, MapCellState state)
    {
        Position = pos;
        MapCellState = state;
    }
}