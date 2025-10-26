using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapGenerationRule generationRule;
    [SerializeField] private MapVisualSet visualSet;

    [Header("Content")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _scrollRect;

    private Dictionary<Vector2Int, Cell> _map;
    private Dictionary<Vector2Int, MapCellState> mapData;

    private EventManager eventManager;
    private MapGenerator generator = new MapGenerator();

    public event Action OnEnterBattle;
    public event Action OnEnterElite;
    public event Action OnEnterBossBattle;
    public event Action OnEnterEvent;
    public event Action OnEnterRest;
    public event Action OnEnterShop;
    public event Action OnEnteTreasure;
    public event Action OnEnterGoal;

    public Vector2Int CurrentCell { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        eventManager = ServiceLocator.Get<EventManager>();
    }

    public void GenerateMap()
    {
        mapData = generator.GenerateMapData(generationRule);
        BuildUpUI();
        SetAllUnclickable();
        SetCurrent(CurrentCell, generator.GetClickable(CurrentCell));
    }

    public void BuildUpUI()
    {
        // ルール設定
        int Width = generationRule.width;
        int Height = generationRule.height;

        // Canvasサイズ取得
        float canvasHeight = _scrollRect.rect.height;
        float canvasWidth = _scrollRect.rect.width;

        // セルサイズ計算
        float cellSize = canvasWidth * (1 - generationRule.horizontalMarginPercent * 2) / Width;

        // StartCellのYオフセット（下部余白）
        float bottomMargin = canvasHeight * generationRule.verticalMarginPercent;
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
            _map.Add(pos, cell);

            // クリック時の処理を共通化
            cell.OnClicked += HandleCellClicked;
            InvokeEnterEvent(cell.Type);
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
            case CellType.Battle: eventManager.StartBattle(); break;
            case CellType.EliteBattle: eventManager.StartEliteBattle(); break;
            case CellType.BossBattle: eventManager.StartBossBattle(); break;
            case CellType.Event: eventManager.StartEvent(); break;
            case CellType.Rest: eventManager.StartRest(); break;
            case CellType.Shop: eventManager.StartShop(); break;
            case CellType.Treasure: eventManager.StartTreasure(); break;
            case CellType.Goal: eventManager.StartGoal(); break;
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

    // 状態を設定
    public void SetMapState(Dictionary<Vector2Int, MapCellState> mapCellStates, Vector2Int cellPos)
    {
        mapData = mapCellStates;
        CurrentCell = cellPos;
    }

    public void MapSave()
    {
        ServiceLocator.Get<SaveManager>().SaveMap(new MapSaveData(mapData, CurrentCell.x, CurrentCell.y, ServiceLocator.Get<EventManager>().EventSaveData));
    }

    private void HandleCellClicked(Vector2Int pos)
    {
        // 現在地を更新
        CurrentCell = pos;

        // クリック処理ルール
        SetAllUnclickable();
        SelectCell(pos);
        SetClickable(generator.GetClickable(pos));
    }

    /// <summary>
    /// セルをクリックされると呼び出される
    /// </summary>
    /// <param name="pos"></param>
    public void OnCellClicked(Vector2Int pos)
    {
        // 現在地を更新
        CurrentCell = pos;

        // クリック処理ルール
        SetAllUnclickable();
        SelectCell(pos);
        SetClickable(generator.GetClickable(pos));
    }

    public void RestoreMap(SaveData saveData)
    {
        mapData = saveData.Map.mapData;
        BuildUpUI();

        SetMapState(
            saveData.Map.mapData,
            new Vector2Int(saveData.Map.cellX, saveData.Map.cellY)
        );

        OnCellClicked(new Vector2Int(saveData.Map.cellX, saveData.Map.cellY));

        // 未完了イベントがあれば再開
        if (saveData.Event != null && !saveData.Event.isCompleted)
        {
            eventManager.LoadData(saveData.Event);
        }
    }

}
