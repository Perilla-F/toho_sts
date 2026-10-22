using System;
using System.Collections.Generic;
using System.Linq;
using Codice.Client.BaseCommands;
using UnityEngine;

public class MapManager
{
    private readonly MapGenerator _generator;
    private IGameManager _gameManager;
    private IGameContext _context;
    private MapGenerationRule rule;

    // イベント（UIに通知）
    public event Action<Dictionary<Vector2Int, MapCellState>, MapGenerationRule> OnMapGenerated;
    public event Action<Vector2Int, IEnumerable<Vector2Int>> OnCellSelectionChanged;

    public MapManager(IGameManager gameManager, IGameContext context, MapGenerationRule rule)
    {
        _gameManager = gameManager;
        _context = context;
        _generator = new MapGenerator(context, rule);
        this.rule = rule;
        _context.SetStageIndex(1);
    }

    public void GenerateMap()
    {
        _context.SetMapData(_generator.GenerateMapData());
        OnMapGenerated?.Invoke(_context.MapData, rule);
        _context.SetCurrentCell(new(rule.width / 2, 0));
        OnCellSelectionChanged?.Invoke(_context.CurrentCell, _generator.GetClickable(_context.CurrentCell));
        _gameManager.RequestSave();
    }

    public void SelectCell(Vector2Int pos)
    {
        _context.SetCurrentCell(pos);
        var clickable = _generator.GetClickable(pos);
        _gameManager.RequestSave();
        OnCellSelectionChanged?.Invoke(pos, clickable);
    }

    public void RestoreFrom()
    {
        OnMapGenerated?.Invoke(_context.MapData, rule);

        OnCellSelectionChanged?.Invoke(_context.CurrentCell, _generator.GetClickable(_context.CurrentCell));
        _gameManager.RequestSave();
    }

}
