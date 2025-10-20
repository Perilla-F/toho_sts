using System;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; set; }

    [Header("Map Settings")]
    [SerializeField] private int Width = 5;
    [SerializeField] private int Height = 10;

    [Header("Event Data")]
    [SerializeField] private EventDatabase EventDatabase;

    public Cell CurrentCell { get; private set; }
    public LastEventData LastEventData { get; set; }
    public Dictionary<Vector2Int, MapCellState> mapData;

    // イベントとしてBridgeに通知する
    public event Action<Dictionary<Vector2Int, MapCellState>> OnMapLoadRequested;
    public event Action OnAutoSaveRequested;
    public event Action<Dictionary<Vector2Int, MapCellState>> OnMapGenerated;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ランダムマップ生成リクエスト
    /// </summary>
    public void GenerateMapData()
    {
        mapData = new Dictionary<Vector2Int, MapCellState>();

        Vector2Int startPos = new(Width / 2, 0);
        mapData[startPos] = new MapCellState(
            startPos,
            false,
            CreateCell(CellType.Start, startPos, true)
        );

        for (int y = 1; y < Height - 1; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                mapData[pos] = new MapCellState(pos, false, CreateCell(RandomCellType(), pos, false));
            }
        }

        Vector2Int goalPos = new(Width / 2, Height - 1);
        mapData[goalPos] = new MapCellState(
            goalPos,
            false,
            CreateCell(CellType.Start, startPos, true)
        ); ;

        CurrentCell.GridPos = startPos;

        // Bridge層に「データができたよ」と通知
        OnMapGenerated?.Invoke(mapData);
    }

    private Cell CreateCell(CellType type, Vector2Int pos, bool isWide = false)
    {
        Cell cell = new Cell();
        if (cell != null)
        {
            cell.Initialize(type, pos, isWide);
            // 初期化
            cell.AssignedEvent = type == CellType.Event ? GetRandomEvent() : null;

            // セルに振る舞いを割り当て
            AssignBehavior(cell, type);

            mapData[pos].Cell = cell;
        }

        return cell;
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

        int rand = UnityEngine.Random.Range(0, total);
        int cumulative = 0;
        foreach (var pair in weights)
        {
            cumulative += pair.Value;
            if (rand < cumulative)
                return pair.Key;
        }

        return CellType.Battle;
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
            case CellType.Shop:
                cell.gameObject.AddComponent<ShopCell>();
                break;
            case CellType.Treasure:
                cell.gameObject.AddComponent<TreasureCell>();
                break;
            case CellType.EliteBattle:
                cell.gameObject.AddComponent<EliteCell>();
                break;
            case CellType.BossBattle:
                cell.gameObject.AddComponent<BossCell>();
                break;
            case CellType.Start:
            case CellType.Goal:
                break;
        }
    }

    private void UpdateSelectableCells()
    {
        foreach (var state in mapData.Values)
            state.Cell.SetSelectable(false);

        Vector2Int cp = CurrentCell.GridPos;
        if (CurrentCell.IsWide)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector2Int next = new(x, cp.y + 1);
                if (mapData.ContainsKey(next))
                    mapData[next].Cell.SetSelectable(true);
            }
        }
        else if (mapData[new Vector2Int(2, cp.y + 1)].Cell.IsWide)
        {
            mapData[new Vector2Int(2, cp.y + 1)].Cell.SetSelectable(true);
        }
        else
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                Vector2Int next = new(cp.x + dx, cp.y + 1);
                if (mapData.ContainsKey(next))
                    mapData[next].Cell.SetSelectable(true);
            }
        }
    }

    public void SelectCell(Cell cell)
    {
        if (cell == null) return;

        cell.Behavior?.OnPlayerEnter();
        CurrentCell.SetCurrent(false);
        CurrentCell = cell;
        CurrentCell.SetCurrent(true);
        UpdateSelectableCells();
    }

    /// <summary>
    /// 座標復元
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Cell GetCellAt(Vector2Int pos)
    {
        return mapData.ContainsKey(pos) ? mapData[pos].Cell : null;
    }

    private MultiStepEvent GetRandomEvent()
    {
        return EventDatabase.GetRandomEvent();
    }

    // Coordinator から状態を設定
    public void SetMapState(Dictionary<Vector2Int, MapCellState> mapCellStates, Vector2Int cellPos, LastEventData lastEventData)
    {
        mapData = mapCellStates;
        CurrentCell = mapCellStates[cellPos].Cell;
        LastEventData = lastEventData == null ? null : lastEventData;
    }

    /// <summary>
    /// イベント開始前に呼ぶ
    /// </summary>
    public void SetLastEvent(string eventId)
    {
        LastEventData = new LastEventData
        {
            eventId = eventId,
            isCompleted = false,
            selectedOptionId = null
        };
    }

    /// <summary>
    /// イベント終了時に呼ばれる
    /// </summary>
    public void CompleteLastEvent(string optionId)
    {
        if (LastEventData == null) return;

        LastEventData.isCompleted = true;
        LastEventData.selectedOptionId = optionId;
    }

    /// <summary>
    /// イベント再開
    /// </summary>
    public void TryResumeLastEvent()
    {
        if (LastEventData != null && !LastEventData.isCompleted)
        {
            var evt = EventDatabase.GetEvent(LastEventData.eventId) as MultiStepEvent;
            EventRunner.Instance.StartEvent(evt);
        }
    }

    public MapSaveData GetSaveData()
    {
        return new MapSaveData
        {
            mapData = mapData,
            cellX = CurrentCell.GridPos.x,
            cellY = CurrentCell.GridPos.y,
        };
    }

    // Bridge層から受け取る
    public void LoadMap(SaveData saveData)
    {
        mapData = saveData.Map.mapData;
        CurrentCell = saveData.Map.mapData[CurrentCell.GridPos].Cell;
        OnMapGenerated?.Invoke(mapData);
        SelectCell(CurrentCell);
    }

    public Dictionary<Vector2Int, MapCellState> GetMapCellStates()
    {
        return mapData;
    }

}
