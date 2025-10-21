using System;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEngine;

public class MapManager
{

    [Header("Map Settings")]
    private int width;
    private int height;

    [Header("Event Data")]
    private EventDatabase eventDatabase;

    public Vector2Int CurrentCell { get; private set; }
    public Dictionary<Vector2Int, MapCellState> mapData;
    public LastEventData LastEventData { get; set; }

    private readonly IMapView mapView;

    public MapManager(IMapView mapView, int width, int height)
    {
        this.mapView = mapView;
        this.width = width;
        this.height = height;
    }

    /// <summary>
    /// ランダムマップ生成リクエスト
    /// </summary>
    public void GenerateMapData(Vector2Int current)
    {
        mapData = new Dictionary<Vector2Int, MapCellState>();

        Vector2Int startPos = new(width / 2, 0);
        MapCellState startCellState = new MapCellState(
            startPos,
            false,
            CellType.Start,
            true
        );
        mapData.Add(startPos, startCellState);

        for (int y = 1; y < height - 2; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                MapCellState cellState = new MapCellState(pos, false, RandomCellType(), false);
                mapData.Add(pos, cellState);
            }
        }

        Vector2Int restPos = new(width / 2, height - 2);
        MapCellState restCellState = new MapCellState(
            restPos,
            false,
            CellType.Rest,
            true
        );
        mapData.Add(restPos, restCellState);

        Vector2Int goalPos = new(width / 2, height - 1);
        MapCellState goalCellState = new MapCellState(
            goalPos,
            false,
            CellType.Goal,
            true
        );
        mapData.Add(goalPos, goalCellState);

        // MapGeneratorへ
        mapView.BuildUpUI(mapData);
        mapView.SetAllUnclickable();
        mapView.SetCurrent(current, GetClickable(current));
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

    /// <summary>
    /// セルをクリックされると呼び出される
    /// </summary>
    /// <param name="pos"></param>
    public void OnCellClicked(Vector2Int pos)
    {
        // 現在地を更新
        CurrentCell = pos;

        // クリック処理ルール
        mapView.SetAllUnclickable();
        mapView.SelectCell(pos);
        mapView.SetClickable(GetClickable(pos));
    }

    private List<Vector2Int> GetClickable(Vector2Int current)
    {
        var list = new List<Vector2Int>();

        if (mapData[current].IsWide)
        {
            list.Add(current + Vector2Int.up);
            list.Add(current + Vector2Int.up + Vector2Int.left);
            list.Add(current + Vector2Int.up + Vector2Int.left + Vector2Int.left);
            list.Add(current + Vector2Int.up + Vector2Int.right);
            list.Add(current + Vector2Int.up + Vector2Int.right + Vector2Int.right);
        }
        else if (mapData[new Vector2Int(2, current.y + 1)].IsWide)
        {
            list.Add(new Vector2Int(2, current.y + 1));
        }
        else
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                Vector2Int next = new(current.x + dx, current.y + 1);
                if (mapData.ContainsKey(next))
                {
                    list.Add(next);
                }
            }
        }
        return list;
    }

    // Coordinator から状態を設定
    public void SetMapState(Dictionary<Vector2Int, MapCellState> mapCellStates, Vector2Int cellPos, LastEventData lastEventData)
    {
        mapData = mapCellStates;
        CurrentCell = cellPos;
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
            stepId = null,
            isCompleted = false,
        };
    }

    /// <summary>
    /// ステップ移動した際に呼ばれる
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="stepId"></param>
    public void StepLastEvent(string eventId, string stepId)
    {
        LastEventData = new LastEventData
        {
            eventId = eventId,
            stepId = stepId,
            isCompleted = false,
        };
    }

    /// <summary>
    /// イベント終了時に呼ばれる
    /// </summary>
    public void CompleteLastEvent()
    {
        if (LastEventData == null) return;
        LastEventData.isCompleted = true;
    }

    public MapSaveData GetSaveData()
    {
        return new MapSaveData
        {
            mapData = mapData,
            cellX = CurrentCell.x,
            cellY = CurrentCell.y,
        };
    }

    public void LoadMap(SaveData saveData)
    {
        mapData = saveData.Map.mapData;
        CurrentCell = saveData.Map.mapData[CurrentCell].GridPos;
        mapView.BuildUpUI(saveData.Map.mapData);
    }

    public Dictionary<Vector2Int, MapCellState> GetMapCellStates()
    {
        return mapData;
    }

}
