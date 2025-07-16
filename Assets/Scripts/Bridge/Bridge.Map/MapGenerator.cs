using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    [SerializeField] public GameObject normalCellPrefab;
    [SerializeField] public GameObject wideCellPrefab;
    [SerializeField] public int width = 5;
    [SerializeField] public int height = 10;
    public float spacing = 1.1f;
    public List<EventBase> eventList;
    public TextAsset jsonFile;
    public EventDatabase eventDatabase;
    public JsonEventList jsonData;

    [SerializeField] public Sprite startIcon, goalIcon, battleIcon, eliteBattleIcon, bossBattleIcon, shopIcon, treasureIcon, restIcon, eventIcon;

    private Dictionary<Vector2Int, Cell> mapCells = new();
    private Cell currentCell;

    [System.Serializable]
    public class JsonEvent
    {
        public string id;
        public string type;
    }

    [System.Serializable]
    public class JsonEventList
    {
        public List<JsonEvent> events;
    }

    void Awake() => Instance = this;

    void Start()
    {
        GenerateMap();
        //Camera.main.GetComponent<CameraFollow>().SetTarget(currentCell.transform);
    }

    void GenerateMap()
    {
        jsonData = JsonUtility.FromJson<JsonEventList>(jsonFile.text);

        // STARTマス
        CreateCell(wideCellPrefab, CellType.Start, new Vector2Int(2, 0), startIcon, true);

        // 通常マス y = 1〜7
        List<Vector2Int> prevRow = new() { new Vector2Int(2, 0) };

        for (int y = 1; y <= height - 3; y++)
        {
            List<Vector2Int> newRow = new();

            foreach (var pos in prevRow)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector2Int newPos = new(x, y);
                    if (!mapCells.ContainsKey(newPos))
                    {
                        var type = RandomCellType();
                        var icon = GetIcon(type);
                        CreateCell(normalCellPrefab, type, newPos, icon);
                        newRow.Add(newPos);
                    }
                }
            }

            prevRow = newRow;
        }

        // RESTマス
        CreateCell(wideCellPrefab, CellType.Rest, new Vector2Int(2, height - 2), restIcon, true);
        // BOSSマス
        CreateCell(wideCellPrefab, CellType.BossBattle, new Vector2Int(2, height - 1), bossBattleIcon, true);

        currentCell = mapCells[new Vector2Int(2, 0)];
        currentCell.SetCurrent(true);
        UpdateSelectableCells();
    }

    void CreateCell(GameObject prefab, CellType type, Vector2Int pos, Sprite icon, bool isWide = false)
    {
        // 横方向の中央寄せオフセットはそのまま
        float xOffset = (width - 1) / 2f * spacing * 2.1f;

        // 縦方向のオフセットを0行目基準に（MapGenerator の位置が y=0行目に一致）
        float yOffset = 0;

        Vector3 offset = new Vector3(xOffset, yOffset, 0);

        // MapGeneratorのtransform.positionを基準にマップを生成
        Vector3 worldPos = transform.position + new Vector3(pos.x * spacing * 2.1f, pos.y * spacing * 2.1f, 0) - offset;

        GameObject obj = Instantiate(prefab, worldPos, Quaternion.identity, transform);
        Cell cell = obj.GetComponent<Cell>();
        AssignBehavior(cell, type);
        cell.Initialize(type, pos, icon, isWide);
        cell.assignedEvent = type == CellType.Event ? GetRandomEvent() : null;
        mapCells[pos] = cell;
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
        int index = Random.Range(0, jsonData.events.Count);
        return eventDatabase.GetEventById(jsonData.events[index].id);
    }

    public void AssignBehavior(Cell cell, CellType type, EventBase assignedEvent = null)
    {
        switch (type)
        {
            case CellType.Event:
                var evt = cell.gameObject.AddComponent<EventCell>();
                evt.assignedEvent = assignedEvent;
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
        CellType.Start => startIcon,
        CellType.Goal => goalIcon,
        CellType.Battle => battleIcon,
        CellType.EliteBattle => eliteBattleIcon,
        CellType.BossBattle => bossBattleIcon,
        CellType.Shop => shopIcon,
        CellType.Treasure => treasureIcon,
        CellType.Rest => restIcon,
        CellType.Event => eventIcon,
        _ => null
    };

    void UpdateSelectableCells()
    {
        foreach (var cell in mapCells.Values)
            cell.SetSelectable(false);

        Vector2Int cp = currentCell.gridPos;
        if (currentCell.isWide)
        {
            for (int x = 0; x < 5; x++)
            {
                Vector2Int next = new(x, cp.y + 1);
                if (mapCells.ContainsKey(next))
                    mapCells[next].SetSelectable(true);
            }
        }
        else if (mapCells[new Vector2Int(2, cp.y + 1)].isWide)
        {
            mapCells[new Vector2Int(2, cp.y + 1)].SetSelectable(true);
        }
        else
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                Vector2Int next = new(cp.x + dx, cp.y + 1);
                if (mapCells.ContainsKey(next))
                    mapCells[next].SetSelectable(true);
            }
        }
    }

    public bool CanSelect(Cell cell)
    {
        return cell.selectableEffect.activeSelf;
    }

    public void SelectCell(Cell cell)
    {
        cell.behaviour?.OnPlayerEnter();
        currentCell.SetCurrent(false);
        currentCell = cell;
        currentCell.SetCurrent(true);
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
