using System;
using UnityEngine;

/// <summary>
/// マップセルの状態保持用
/// </summary>
[Serializable]
public class MapCellState
{
    public Vector2Int GridPos;
    public bool Cleared;
    public Cell Cell;

    public MapCellState(Vector2Int gridPos, bool cleared, Cell cell)
    {
        GridPos = gridPos;
        Cleared = cleared;
        Cell = cell;
    }
}
