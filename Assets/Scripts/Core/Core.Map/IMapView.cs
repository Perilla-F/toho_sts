using System.Collections.Generic;
using UnityEngine;

public interface IMapView
{
    void BuildUpUI(Dictionary<Vector2Int, MapCellState> mapData);
    void SelectCell(Vector2Int position);
    void SetAllUnclickable();
    void SetClickable(IEnumerable<Vector2Int> positions);
    void SetCurrent(Vector2Int current, IEnumerable<Vector2Int> positions);
}