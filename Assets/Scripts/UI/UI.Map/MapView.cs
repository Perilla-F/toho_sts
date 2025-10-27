using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MapView : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform scrollRect;
    [SerializeField] private MapVisualSet visualSet;

    private Dictionary<Vector2Int, Cell> _cells = new();

    public event Action<Vector2Int, CellType> OnCellClicked;

    public void BuildMapUI(Dictionary<Vector2Int, MapCellState> mapData, MapGenerationRule rule)
    {
        // ルール設定
        int Width = rule.width;
        int Height = rule.height;

        // Canvasサイズ取得
        float canvasHeight = scrollRect.rect.height;
        float canvasWidth = scrollRect.rect.width;

        // セルサイズ計算
        float cellSize = canvasWidth * (1 - rule.horizontalMarginPercent * 2) / Width;

        // StartCellのYオフセット（下部余白）
        float bottomMargin = canvasHeight * rule.verticalMarginPercent;
        float totalMapHeight = cellSize * Height;
        float startYOffset = -totalMapHeight / 2f + bottomMargin;

        foreach (var kvp in mapData)
        {
            var pos = kvp.Key;
            var state = kvp.Value;

            // Prefab選択
            var prefab = state.IsWide ? visualSet.WideCellPrefab : visualSet.NormalCellPrefab;

            // Cell生成
            var cell = BuildCell(prefab, state, startYOffset, cellSize, Width, state.IsWide);
            _cells.Add(pos, cell);

            // クリック時の処理を共通化
            cell.OnClicked += (pos, type) => OnCellClicked?.Invoke(pos, type);
        }
        // Content の高さ調整
        AdjustContentSize(totalMapHeight, bottomMargin);
    }

    private Cell BuildCell(GameObject prefab, MapCellState state, float yOffset, float size, int width, bool isWide = false)
    {
        GameObject obj = Instantiate(prefab, content);
        RectTransform rt = obj.GetComponent<RectTransform>();

        // サイズを調整
        if (isWide)
        {
            rt.sizeDelta = new Vector2(size * 5f, size);
        }
        else
        {
            rt.sizeDelta = new Vector2(size, size);
        }

        // 座標を計算（中央揃え）
        float xOffset = size * (width - 1) / 2f;
        rt.anchoredPosition = new Vector2(
            state.GridPos.x * size - xOffset,
            state.GridPos.y * size + yOffset
            );

        Cell cell = obj.GetComponent<Cell>();
        cell.Initialize(state.Type, state.GridPos, isWide);
        cell.SetIcon(GetIcon(state.Type));

        return cell;
    }

    private Sprite GetIcon(CellType type) => type switch
    {
        CellType.Start => visualSet.StartIcon,
        CellType.Goal => visualSet.GoalIcon,
        CellType.Battle => visualSet.BattleIcon,
        CellType.EliteBattle => visualSet.EliteIcon,
        CellType.BossBattle => visualSet.BossIcon,
        CellType.Shop => visualSet.ShopIcon,
        CellType.Treasure => visualSet.TreasureIcon,
        CellType.Rest => visualSet.RestIcon,
        CellType.Event => visualSet.EventIcon,
        _ => null
    };

    /// <summary>
    /// Content の高さをセル数に応じて調整
    /// </summary>
    void AdjustContentSize(float totalHeight, float margin)
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, totalHeight + 2f * margin);
    }

    public void UpdateCellSelection(Vector2Int current, IEnumerable<Vector2Int> selectable)
    {
        foreach (var cell in _cells.Values)
        {
            cell.SetCurrent(cell.GridPos == current);
            cell.SetSelectable(selectable.Contains(cell.GridPos));
        }
    }

}
