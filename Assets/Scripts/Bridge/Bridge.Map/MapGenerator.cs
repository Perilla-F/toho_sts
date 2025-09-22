using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    [SerializeField] public GameObject NormalCellPrefab;
    [SerializeField] public GameObject WideCellPrefab;
    [SerializeField] public int Width = 5;
    [SerializeField] public int Height = 10;
    public float Spacing = 1.1f;
    public List<EventBase> EventList;
    public TextAsset JsonFile;
    public EventDatabase EventDatabase;
    public JsonEventList JsonData;

    [SerializeField] public Sprite StartIcon, GoalIcon, BattleIcon, EliteBattleIcon, BossBattleIcon, ShopIcon, TreasureIcon, RestIcon, EventIcon;

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

    void Awake() => Instance = this;

    void Start()
    {
        GenerateMap();
        //Camera.main.GetComponent<CameraFollow>().SetTarget(currentCell.transform);
    }

    void GenerateMap()
    {
        JsonData = JsonUtility.FromJson<JsonEventList>(JsonFile.text);

        // STARTマス
        CreateCell(WideCellPrefab, CellType.Start, new Vector2Int(2, 0), StartIcon, true);

        // 通常マス y = 1〜7
        List<Vector2Int> prevRow = new() { new Vector2Int(2, 0) };

        for (int y = 1; y <= Height - 3; y++)
        {
            List<Vector2Int> newRow = new();

            foreach (var pos in prevRow)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector2Int newPos = new(x, y);
                    if (!_mapCells.ContainsKey(newPos))
                    {
                        var type = RandomCellType();
                        var icon = GetIcon(type);
                        CreateCell(NormalCellPrefab, type, newPos, icon);
                        newRow.Add(newPos);
                    }
                }
            }

            prevRow = newRow;
        }

        // RESTマス
        CreateCell(WideCellPrefab, CellType.Rest, new Vector2Int(2, Height - 2), RestIcon, true);
        // BOSSマス
        CreateCell(WideCellPrefab, CellType.BossBattle, new Vector2Int(2, Height - 1), BossBattleIcon, true);

        _currentCell = _mapCells[new Vector2Int(2, 0)];
        _currentCell.SetCurrent(true);
        UpdateSelectableCells();
    }

    void CreateCell(GameObject prefab, CellType type, Vector2Int pos, Sprite icon, bool isWide = false)
    {
        // 横方向の中央寄せオフセットはそのまま
        float xOffset = (Width - 1) / 2f * Spacing * 2.1f;

        // 縦方向のオフセットを0行目基準に（MapGenerator の位置が y=0行目に一致）
        float yOffset = 0;

        Vector3 offset = new Vector3(xOffset, yOffset, 0);

        // MapGeneratorのtransform.positionを基準にマップを生成
        Vector3 worldPos = transform.position + new Vector3(pos.x * Spacing * 2.1f, pos.y * Spacing * 2.1f, 0) - offset;

        GameObject obj = Instantiate(prefab, worldPos, Quaternion.identity, transform);
        Cell cell = obj.GetComponent<Cell>();
        AssignBehavior(cell, type);
        cell.Initialize(type, pos, icon, isWide);
        cell.AssignedEvent = type == CellType.Event ? GetRandomEvent() : null;
        _mapCells[pos] = cell;
    }

    CellType RandomCellType()
    {
        Dictionary<CellType, int> weights = new()
    {
        { CellType.Battle, 50 },
        { CellType.EliteBattle, 20 },
        { CellType.Shop, 10 },
        { CellType.Treasure,10 },
        { CellType.Rest, 5 },
        { CellType.Event, 20 },
    };

        int totalWeight = 0;
        foreach (var w in weights.Values) totalWeight += w;

        int rand = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var pair in weights)
        {
            cumulative += pair.Value;
            if (rand < cumulative)
                return pair.Key;
        }

        return CellType.Battle; // フォールバック
    }

    EventBase GetRandomEvent()
    {
        int index = Random.Range(0, JsonData.Events.Count);
        return EventDatabase.GetEventById(JsonData.Events[index].Id);
    }

    public void AssignBehavior(Cell cell, CellType type, EventBase assignedEvent = null)
    {
        switch (type)
        {
            case CellType.Event:
                var evt = cell.gameObject.AddComponent<EventCell>();
                evt.AssignedEvent = assignedEvent;
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

    Sprite GetIcon(CellType type) => type switch
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

    void UpdateSelectableCells()
    {
        foreach (var cell in _mapCells.Values)
            cell.SetSelectable(false);

        Vector2Int cp = _currentCell.GridPos;
        if (_currentCell.IsWide)
        {
            for (int x = 0; x < 5; x++)
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

    public bool CanSelect(Cell cell)
    {
        return cell.SelectableEffect.activeSelf;
    }

    public void SelectCell(Cell cell)
    {
        cell.Behaviour?.OnPlayerEnter();
        _currentCell.SetCurrent(false);
        _currentCell = cell;
        _currentCell.SetCurrent(true);
        UpdateSelectableCells();
        // TODO: イベント処理や戦闘遷移
        // Camera.main.GetComponent<CameraFollow>().SetTarget(currentCell.transform);
    }

    public void StartEvent(EventBase assingedEvent)
    {
        assingedEvent.Execute();
    }

    public void StartRest()
    { }

    public void StartShop()
    { }

    public void StartTreasure()
    { }

    public void StartBattle()
    { }

    public void StartBoss()
    { }
}
