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
    public event Action OnEnterBattle;
    public event Action OnEnterElite;
    public event Action OnEnterBossBattle;
    public event Action OnEnterEvent;
    public event Action OnEnterRest;
    public event Action OnEnterShop;
    public event Action OnEnteTreasure;
    public event Action OnEnterGoal;

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

        foreach (var kvp in mapData)
        {
            var pos = kvp.Key;
            var state = kvp.Value;

            // Prefab選択
            var prefab = state.IsWide ? WideCellPrefab : NormalCellPrefab;

            // Cell生成
            var cell = BuildCell(prefab, state, startYOffset, cellSize, Width, state.IsWide);
            _map.Add(pos, cell);

            // クリック時の処理を共通化
            cell.OnClicked += clickedPos =>
            {
                OnCellClicked?.Invoke(clickedPos);
                InvokeEnterEvent(cell.Type);
            };
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

        return cell;
    }

    /// <summary>
    /// CellType に応じて対応イベントを呼び出す
    /// </summary>
    private void InvokeEnterEvent(CellType type)
    {
        switch (type)
        {
            case CellType.Battle: OnEnterBattle?.Invoke(); break;
            case CellType.EliteBattle: OnEnterElite?.Invoke(); break;
            case CellType.BossBattle: OnEnterBossBattle?.Invoke(); break;
            case CellType.Event: OnEnterEvent?.Invoke(); break;
            case CellType.Rest: OnEnterRest?.Invoke(); break;
            case CellType.Shop: OnEnterShop?.Invoke(); break;
            case CellType.Treasure: OnEnteTreasure?.Invoke(); break;
            case CellType.Goal: OnEnterGoal?.Invoke(); break;
        }
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

    public void SelectCell(Vector2Int pos)
    {
        if (pos == null) return;
        _map[pos].SetCurrent(true);
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
