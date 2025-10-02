using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    [Header("Cell Prefabs")]
    [SerializeField] private GameObject NormalCellPrefab;
    [SerializeField] private GameObject WideCellPrefab;

    [Header("Map Settings")]
    [SerializeField] private int Width = 5;
    [SerializeField] private int Height = 10;
    [SerializeField] private float Spacing = 150f;
    [SerializeField] private float VMargin = 50f;
    [SerializeField] private float verticalMarginPercent = 0.1f; // 画面縦の余白割合
    [SerializeField] private float horizontalMarginPercent = 0.05f; // 横余白割合

    [Header("Content")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _scrollRect;
    [SerializeField] private RectTransform _playerMarker;

    [Header("Icons")]
    [SerializeField] private Sprite StartIcon, GoalIcon, BattleIcon, EliteBattleIcon, BossBattleIcon, ShopIcon, TreasureIcon, RestIcon, EventIcon;

    [Header("Event Data")]
    [SerializeField] private EventDatabase EventDatabase;
    [SerializeField] private TextAsset JsonFile;

    private Dictionary<Vector2Int, Cell> _mapCells = new();
    private Cell _currentCell;

    [System.Serializable]
    public class JsonEvent
    {
        public string Id;
        public string Type;
    }

    [System.Serializable]
    public class JsonEventList
    {
        public List<JsonEvent> Events;
    }

    private JsonEventList JsonData;

    private void Awake() => Instance = this;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        _mapCells.Clear();

        // Canvasサイズ取得
        float canvasHeight = _scrollRect.rect.height;
        float canvasWidth = _scrollRect.rect.width;

        // セルサイズ計算
        float cellSize = canvasWidth * (1 - horizontalMarginPercent * 2) / Width;

        // StartCellのYオフセット（下部余白）
        float bottomMargin = canvasHeight * verticalMarginPercent;
        float totalMapHeight = cellSize * Height;
        float startYOffset = -totalMapHeight / 2f + bottomMargin;

        // 1. Startセル固定
        Vector2Int startPos = new(Width / 2, 0);
        var icon = GetIcon(CellType.Start);
        CreateCell(WideCellPrefab, CellType.Start, startPos, icon, startYOffset, cellSize, true);

        // 2. 通常/ランダムセル生成
        for (int y = 1; y < Height - 2; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector2Int pos = new(x, y);

                // ランダムタイプを取得
                var type = RandomCellType();
                icon = GetIcon(type);
                CreateCell(NormalCellPrefab, type, pos, icon, startYOffset, cellSize, false);
            }
        }
        // 3. Restセル固定
        Vector2Int restPos = new(Width / 2, Height - 2);
        icon = GetIcon(CellType.Rest);
        CreateCell(WideCellPrefab, CellType.Rest, restPos, icon, startYOffset, cellSize, true);

        // 4. Goalセル固定
        Vector2Int goalPos = new(Width / 2, Height - 1);
        icon = GetIcon(CellType.Goal);
        CreateCell(WideCellPrefab, CellType.Goal, goalPos, icon, startYOffset, cellSize, true);

        _currentCell = _mapCells[new Vector2Int(2, 0)];
        SelectCell(_currentCell);

        UpdateSelectableCells();

        // Content の高さ調整
        AdjustContentSize(totalMapHeight, bottomMargin);

    }

    private void CreateCell(GameObject prefab, CellType type, Vector2Int pos, Sprite icon, float yOffset, float size, bool isWide = false)
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
        float xOffset = size * (Width - 1) / 2f;
        rt.anchoredPosition = new Vector2(pos.x * size - xOffset,
                                          pos.y * size + yOffset);

        Cell cell = obj.GetComponent<Cell>();
        if (cell != null)
        {
            cell.Initialize(type, pos, icon, isWide);

            // タイプに応じて振る舞いをアタッチ
            switch (type)
            {
                case CellType.Battle:
                case CellType.EliteBattle:
                    cell.Behavior = obj.AddComponent<BattleCell>();
                    break;
                case CellType.Rest:
                    cell.Behavior = obj.AddComponent<RestCell>();
                    break;
                case CellType.Shop:
                    cell.Behavior = obj.AddComponent<ShopCell>();
                    break;
                case CellType.Treasure:
                    cell.Behavior = obj.AddComponent<TreasureCell>();
                    break;
                case CellType.Event:
                    cell.Behavior = obj.AddComponent<EventCell>();
                    break;
                case CellType.BossBattle:
                    cell.Behavior = obj.AddComponent<BossCell>();
                    break;
                case CellType.Start:
                case CellType.Goal:
                    break;
            }

            // // セルに振る舞いを割り当て
            // AssignBehavior(cell, type);

            // 初期化
            // cell.AssignedEvent = type == CellType.Event ? GetRandomEvent() : null;

            _mapCells[pos] = cell;
        }
    }

    private CellType RandomCellType()
    {
        Dictionary<CellType, int> weights = new()
        {
            { CellType.Battle, 50 },
            { CellType.EliteBattle, 20 },
            { CellType.Shop, 10 },
            { CellType.Treasure, 10 },
            { CellType.Rest, 5 },
            { CellType.Event, 20 },
        };

        int total = 0;
        foreach (var w in weights.Values) total += w;

        int rand = Random.Range(0, total);
        int cumulative = 0;
        foreach (var pair in weights)
        {
            cumulative += pair.Value;
            if (rand < cumulative)
                return pair.Key;
        }

        return CellType.Battle;
    }

    public RectTransform GetStartCellRect()
    {
        foreach (var cell in _mapCells.Values)
        {
            if (cell.Type == CellType.Start)
            {
                return cell.GetComponent<RectTransform>();
            }
        }
        return null;
    }

    /// <summary>
    /// Content の高さをセル数に応じて調整
    /// </summary>
    void AdjustContentSize(float totalHeight, float margin)
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, totalHeight + 2f * margin);
    }

    private EventBase GetRandomEvent()
    {
        int index = Random.Range(0, JsonData.Events.Count);
        return EventDatabase.GetEventById(JsonData.Events[index].Id);
    }

    private void AssignBehavior(Cell cell, CellType type)
    {
        switch (type)
        {
            case CellType.Event:
                var evt = cell.gameObject.AddComponent<EventCell>();
                evt.AssignedEvent = cell.AssignedEvent;
                break;
            case CellType.Rest:
                cell.gameObject.AddComponent<RestCell>();
                break;
            case CellType.Battle:
                cell.gameObject.AddComponent<BattleCell>();
                break;
            case CellType.BossBattle:
                cell.gameObject.AddComponent<BossCell>();
                break;
        }
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

    private void UpdateSelectableCells()
    {
        foreach (var cell in _mapCells.Values)
            cell.SetSelectable(false);

        Vector2Int cp = _currentCell.GridPos;
        if (_currentCell.IsWide)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector2Int next = new(x, cp.y + 1);
                if (_mapCells.ContainsKey(next))
                    _mapCells[next].SetSelectable(true);
            }
        }
        else if (_mapCells[new Vector2Int(2, cp.y + 1)].IsWide)
        {
            _mapCells[new Vector2Int(2, cp.y + 1)].SetSelectable(true);
        }
        else
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                Vector2Int next = new(cp.x + dx, cp.y + 1);
                if (_mapCells.ContainsKey(next))
                    _mapCells[next].SetSelectable(true);
            }
        }
    }

    public void SelectCell(Cell cell)
    {
        cell.Behavior?.OnPlayerEnter();
        _currentCell.SetCurrent(false);
        _currentCell = cell;
        _currentCell.SetCurrent(true);
        UpdateSelectableCells();

        UpdateMarkerPosition();
    }

    private void UpdateMarkerPosition()
    {
        if (_playerMarker != null && _currentCell != null)
        {
            _playerMarker.anchoredPosition = _currentCell.GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
