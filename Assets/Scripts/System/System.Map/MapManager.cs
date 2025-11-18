using System;
using System.Collections.Generic;
using System.Linq;
using Codice.Client.BaseCommands;
using UnityEngine;

public class MapManager
{
    private readonly MapGenerator _generator;
    private GameManager _gameManager;
    private GameContext _context;
    private Dictionary<Vector2Int, MapCellState> _mapData;
    private MapGenerationRule rule;

    public Vector2Int CurrentCell { get; private set; }

    public int StageIndex;

    // イベント（UIに通知）
    public event Action<Dictionary<Vector2Int, MapCellState>, MapGenerationRule> OnMapGenerated;
    public event Action<Vector2Int, IEnumerable<Vector2Int>> OnCellSelectionChanged;

    public MapManager(GameManager gameManager, GameContext context, MapGenerationRule rule)
    {
        _gameManager = gameManager;
        _context = context;
        _generator = new MapGenerator();
        this.rule = rule;
        StageIndex = 1;
    }

    public void Inject(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void GenerateMap()
    {
        _context.SetMapData(_generator.GenerateMapData(rule));
        OnMapGenerated?.Invoke(_mapData, rule);
        CurrentCell = new(rule.width / 2, 0);
        OnCellSelectionChanged?.Invoke(CurrentCell, _generator.GetClickable(CurrentCell));
        _gameManager.RequestSave();
    }

    public void SelectCell(Vector2Int pos)
    {
        CurrentCell = pos;
        var clickable = _generator.GetClickable(pos);
        _gameManager.RequestSave();
        OnCellSelectionChanged?.Invoke(pos, clickable);
    }

    public MapSaveData CreateSaveData()
    {
        return new MapSaveData(_mapData, CurrentCell.x, CurrentCell.y, StageIndex);
    }

    public void RestoreFrom(MapSaveData data)
    {
        _mapData = data.mapData;
        OnMapGenerated?.Invoke(_mapData, rule);

        CurrentCell = new Vector2Int(data.cellX, data.cellY);
        OnCellSelectionChanged?.Invoke(CurrentCell, _generator.GetClickable(CurrentCell));
        _gameManager.RequestSave();
    }

}
