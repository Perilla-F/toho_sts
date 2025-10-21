using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class MapGenerator : MonoBehaviour, IMapView
{
    public static MapGenerator Instance;

    [Header("Cell Prefabs")]
    [SerializeField] private GameObject NormalCellPrefab;
    [SerializeField] private GameObject WideCellPrefab;

    [Header("Content")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _scrollRect;
    /// <summary>
    /// 画面縦の余白割合
    /// </summary>
    [SerializeField] private float verticalMarginPercent = 0.1f;
    /// <summary>
    /// 画面横の余白割合
    /// </summary>
    [SerializeField] private float horizontalMarginPercent = 0.05f;

    [Header("Icons")]
    [SerializeField] private Sprite StartIcon, GoalIcon, BattleIcon, EliteBattleIcon, BossBattleIcon, ShopIcon, TreasureIcon, RestIcon, EventIcon;

    private Dictionary<Vector2Int, Cell> _map;

    public event Action<Vector2Int> OnCellClicked;

    private void Awake() => Instance = this;

    public void BuildUpUI(Dictionary<Vector2Int, MapCellState> mapData)
    {
        _map = new Dictionary<Vector2Int, Cell>();
        int Width = int.MinValue;
        int Height = int.MinValue;
        foreach (var pos in mapData.Keys)
        {
            if (pos.x > Width) Width = pos.x;
            if (pos.y > Height) Height = pos.y;
        }
        Width++;
        Height++;

        // Canvasサイズ取得
        float canvasHeight = _scrollRect.rect.height;
        float canvasWidth = _scrollRect.rect.width;

        // セルサイズ計算
        float cellSize = canvasWidth * (1 - horizontalMarginPercent * 2) / Width;

        // StartCellのYオフセット（下部余白）
        float bottomMargin = canvasHeight * verticalMarginPercent;
        float totalMapHeight = cellSize * Height;
        float startYOffset = -totalMapHeight / 2f + bottomMargin;

        foreach (var pos in mapData.Keys)
        {

            if (mapData[pos].IsWide)
            {
                var cell = BuildCell(WideCellPrefab, mapData[pos], startYOffset, cellSize, Width, true);
                _map.Add(pos, cell);
                cell.OnClicked += pos => OnCellClicked?.Invoke(pos);
            }
            else
            {
                var cell = BuildCell(NormalCellPrefab, mapData[pos], startYOffset, cellSize, Width, false);
                _map.Add(pos, cell);
                cell.OnClicked += pos => OnCellClicked?.Invoke(pos);
            }
        }

        // Content の高さ調整
        AdjustContentSize(totalMapHeight, bottomMargin);
    }

    private Cell BuildCell(GameObject prefab, MapCellState state, float yOffset, float size, int width, bool isWide = false)
    {
        GameObject obj = Instantiate(prefab, _content);
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

        if (cell != null)
        {
            AssignBehavior(cell, state);
        }
        return cell;
    }

    /// <summary>
    /// Content の高さをセル数に応じて調整
    /// </summary>
    void AdjustContentSize(float totalHeight, float margin)
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, totalHeight + 2f * margin);
    }

    private Sprite GetIcon(CellType type) => type switch
    {
        CellType.Start => StartIcon,
        CellType.Goal => GoalIcon,
        CellType.Battle => BattleIcon,
        CellType.EliteBattle => EliteBattleIcon,
        CellType.BossBattle => BossBattleIcon,
        CellType.Shop => ShopIcon,
        CellType.Treasure => TreasureIcon,
        CellType.Rest => RestIcon,
        CellType.Event => EventIcon,
        _ => null
    };

    /// <summary>
    /// セルに振る舞いを割り当て
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="type"></param>
    private void AssignBehavior(Cell cell, MapCellState state)
    {
        switch (state.Type)
        {
            case CellType.Event:
                cell.Behavior = new EventCell();
                cell.Behavior.AssignedEvent = state.AssignedEvent;
                break;
            case CellType.Rest:
                cell.Behavior = new RestCell();
                break;
            case CellType.Battle:
                cell.Behavior = new BattleCell();
                break;
            case CellType.EliteBattle:
                cell.Behavior = new EliteCell();
                break;
            case CellType.BossBattle:
                cell.Behavior = new BossCell();
                break;
            case CellType.Shop:
                cell.Behavior = new ShopCell();
                break;
            case CellType.Treasure:
                cell.Behavior = new TreasureCell();
                break;
            case CellType.Start:
            case CellType.Goal:
                break;
        }
    }

    public void SelectCell(Vector2Int pos)
    {
        if (pos == null) return;
        _map[pos].SetCurrent(true);
        _map[pos].Behavior?.OnPlayerEnter();
    }

    public void SetClickable(IEnumerable<Vector2Int> positions)
    {
        foreach (var cell in _map.Values)
        {
            cell.SetSelectable(positions.Contains(cell.GridPos));
        }
    }

    public void SetAllUnclickable()
    {
        foreach (var cell in _map.Values)
        {
            cell.SetCurrent(false);
            cell.IsSelectable = false;
        }
    }

    public void SetCurrent(Vector2Int current, IEnumerable<Vector2Int> positions)
    {
        foreach (var cell in _map.Values)
        {
            cell.SetCurrent(false);
        }
        SetAllUnclickable();
        _map[current].SetCurrent(true);
        SetClickable(positions);
    }

}
