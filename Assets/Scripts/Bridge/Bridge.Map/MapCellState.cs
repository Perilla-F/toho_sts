using System;
using UnityEngine;

/// <summary>
/// マップセルのデータ保持用
/// </summary>
[Serializable]
public class MapCellState
{
    public Vector2Int GridPos;
    public bool Cleared;
    public CellType Type;
    public bool IsWide;
    public MultiStepEvent AssignedEvent;

    public MapCellState(Vector2Int gridPos, bool cleared, CellType type, bool isWide)
    {
        GridPos = gridPos;
        Cleared = cleared;
        Type = type;
        IsWide = isWide;
    }
}
