using System;
using System.Collections.Generic;
using System.Linq;
using Codice.Client.BaseCommands;
using UnityEngine;

public class MapManager
{
    private readonly MapGenerator _generator;
    private readonly GameManager _gameManager;
    private Dictionary<Vector2Int, MapCellState> _mapData;

    public Vector2Int CurrentCell { get; private set; }

    // イベント（UIに通知）
    public event Action<Dictionary<Vector2Int, MapCellState>, MapGenerationRule> OnMapGenerated;
    public event Action<Vector2Int, IEnumerable<Vector2Int>> OnCellSelectionChanged;

    public MapManager(GameManager gameManager)
    {
        _gameManager = gameManager;
        _generator = new MapGenerator();
    }

    public void GenerateMap(MapGenerationRule rule)
    {
        _mapData = _generator.GenerateMapData(rule);
        OnMapGenerated?.Invoke(_mapData, rule);
    }

    public void SelectCell(Vector2Int pos)
    {
        CurrentCell = pos;
        var clickable = _generator.GetClickable(pos);
        MapSave();
        OnCellSelectionChanged?.Invoke(pos, clickable);
    }

    public void MapSave()
    {
        _gameManager.SaveMap(new MapSaveData(_mapData, CurrentCell.x, CurrentCell.y));
    }

    public void RestoreMap(SaveData saveData, MapGenerationRule rule)
    {
        _mapData = saveData.Map.mapData;
        OnMapGenerated?.Invoke(_mapData, rule);

        CurrentCell = new Vector2Int(saveData.Map.cellX, saveData.Map.cellY);
        OnCellSelectionChanged?.Invoke(CurrentCell, _generator.GetClickable(CurrentCell));
    }

}
