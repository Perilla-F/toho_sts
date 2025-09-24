using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    [Header("Prefabs")]
    [SerializeField] public GameObject NormalCellPrefab;
    [SerializeField] public GameObject WideCellPrefab;

    [Header("Map Settings")]
    [SerializeField] public int Width = 5;
    [SerializeField] public int Height = 10;
    [SerializeField] public float Spacing = 1.1f;

    [Header("Icons")]
    public Sprite StartIcon, GoalIcon, BattleIcon, EliteBattleIcon, BossBattleIcon, ShopIcon, TreasureIcon, RestIcon, EventIcon;

    [Header("Encounter Data")]
    public List<EncounterData> NormalEncounters;
    public List<EncounterData> EliteEncounters;
    public EncounterData BossEncounter;

    private Dictionary<Vector2Int, Cell> _mapCells = new();
    private Cell _currentCell;

    void Awake() => Instance = this;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        // STARTマス
        CreateCell(WideCellPrefab, CellType.Start, new Vector2Int(2, 0), StartIcon, true);

        List<Vector2Int> prevRow = new() { new Vector2Int(2, 0) };

        for (int y = 1; y <= Height - 3; y++)
        {
            List<Vector2Int> newRow = new();

            // 行単位で Normal/Elite の配置を決める
            List<CellType> rowTypes = GetRowTypes(Width);

            for (int x = 0; x < Width; x++)
            {
                Vector2Int pos = new(x, y);
                if (_mapCells.ContainsKey(pos)) continue;

                CellType type = rowTypes[x];
                GameObject prefab = NormalCellPrefab;
                if (type == CellType.EliteBattle) prefab = WideCellPrefab;

                // CreateCell 内で BattleCell/EliteCell を自動割り当て
                Cell cell = CreateCell(prefab, type, pos, GetIcon(type));

                // Encounterを割り当て
                if (type == CellType.Battle) cell.AssignedEncounter = GetRandomNormalEncounter();
                else if (type == CellType.EliteBattle) cell.AssignedEncounter = GetRandomEliteEncounter();
            }

            prevRow = newRow;
        }

        // RESTマス
        CreateCell(WideCellPrefab, CellType.Rest, new Vector2Int(2, Height - 2), RestIcon, true);

        // BOSSマス
        Cell bossCell = CreateCell(WideCellPrefab, CellType.BossBattle, new Vector2Int(2, Height - 1), BossBattleIcon, true);
        bossCell.AssignedEncounter = BossEncounter;

        _currentCell = _mapCells[new Vector2Int(2, 0)];
        _currentCell.SetCurrent(true);
        UpdateSelectableCells();
    }

    /// <summary>
    /// 行単位で Normal/Elite の配置を決める
    /// 例: Elite 1個、Normal は残り
    /// </summary>
    List<CellType> GetRowTypes(int width)
    {
        List<CellType> types = new() { CellType.EliteBattle };
        for (int i = 1; i < width; i++) types.Add(CellType.Battle);

        // シャッフル
        types = types.OrderBy(x => Random.value).ToList();
        return types;
    }

    Cell CreateCell(GameObject prefab, CellType type, Vector2Int pos, Sprite icon, bool isWide = false)
    {
        float xOffset = (Width - 1) / 2f * Spacing * 2.1f;
        float yOffset = 0;
        Vector3 offset = new Vector3(xOffset, yOffset, 0);
        Vector3 worldPos = transform.position + new Vector3(pos.x * Spacing * 2.1f, pos.y * Spacing * 2.1f, 0) - offset;

        GameObject obj = Instantiate(prefab, worldPos, Quaternion.identity, transform);
        Cell cell = obj.GetComponent<Cell>();
        AssignBehavior(cell, type);
        cell.Initialize(type, pos, icon, isWide);
        _mapCells[pos] = cell;
        return cell;
    }

    public void AssignBehavior(Cell cell, CellType type)
    {
        switch (type)
        {
            case CellType.Battle:
                cell.gameObject.AddComponent<BattleCell>();
                break;
            case CellType.EliteBattle:
                cell.gameObject.AddComponent<EliteCell>();
                break;
            case CellType.BossBattle:
                cell.gameObject.AddComponent<BossCell>();
                break;
            case CellType.Rest:
                cell.gameObject.AddComponent<RestCell>();
                break;
            case CellType.Event:
                var evt = cell.gameObject.AddComponent<EventCell>();
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

    EncounterData GetRandomNormalEncounter()
    {
        if (NormalEncounters.Count == 0) return null;
        int index = Random.Range(0, NormalEncounters.Count);
        return NormalEncounters[index];
    }

    EncounterData GetRandomEliteEncounter()
    {
        if (EliteEncounters.Count == 0) return null;
        int index = Random.Range(0, EliteEncounters.Count);
        EncounterData encounter = EliteEncounters[index];
        EliteEncounters.RemoveAt(index); // 同じEliteが再度出ないように削除
        return encounter;
    }

    void UpdateSelectableCells()
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
    public bool CanSelect(Cell cell)
    {
        return cell.SelectableEffect.activeSelf;
    }

    public void SelectCell(Cell cell)
    {
        if (!CanSelect(cell)) return;

        // 現在セルの更新
        _currentCell.SetCurrent(false);
        _currentCell = cell;
        _currentCell.SetCurrent(true);

        UpdateSelectableCells();

        // セルの種類に応じてバトルやイベント開始
        switch (cell.Type)
        {
            case CellType.Battle:
            case CellType.EliteBattle:
            case CellType.BossBattle:
                StartBattle(cell.AssignedEncounter);
                break;
            case CellType.Rest:
                StartRest();
                break;
            case CellType.Shop:
                StartShop();
                break;
            case CellType.Treasure:
                StartTreasure();
                break;
            case CellType.Event:
                StartEvent(cell.AssignedEvent);
                break;
        }
    }

    public void StartEvent(EventBase assignedEvent) { }
    public void StartRest() { }
    public void StartShop() { }
    public void StartTreasure() { }
    public void StartBattle(EncounterData encounter)
    {
        if (encounter == null) return;

        // BattleManagerなどに渡して戦闘開始
        //        BattleStarter.Instance.StartBattle(encounter);
    }
    public void StartElite() { }
    public void StartBoss() { }
}
