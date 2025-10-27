using System.Collections.Generic;
using UnityEngine;

public class MapPresenter
{
    private readonly GameManager _gameManager;
    private readonly MapManager _mapManager;
    private readonly EventManager _eventManager;
    private readonly MapView _mapView;

    public MapPresenter(GameManager game, MapManager manager, EventManager eventManager, MapView view)
    {
        _gameManager = game;
        _mapManager = manager;
        _eventManager = eventManager;
        _mapView = view;

        _mapManager.OnMapGenerated += OnMapGenerated;
        _mapManager.OnCellSelectionChanged += OnCellSelectionChanged;
        _mapView.OnCellClicked += HandleCellClicked;
    }

    private void OnMapGenerated(Dictionary<Vector2Int, MapCellState> mapData, MapGenerationRule rule)
    {
        _mapView.BuildMapUI(mapData, rule);
    }

    private void OnCellSelectionChanged(Vector2Int current, IEnumerable<Vector2Int> selectable)
    {
        _mapView.UpdateCellSelection(current, selectable);
    }

    private void HandleCellClicked(Vector2Int pos, CellType type)
    {
        // MapManagerに選択を通知
        _mapManager.SelectCell(pos);

        // CellTypeに応じたアクション
        switch (type)
        {
            case CellType.Battle:
                _gameManager.StartBattle(EnemyType.Normal);
                break;
            case CellType.EliteBattle:
                _gameManager.StartBattle(EnemyType.Elite);
                break;
            case CellType.BossBattle:
                _gameManager.StartBattle(EnemyType.Boss);
                break;
            case CellType.Event:
                _eventManager.OnEnterEvent();
                break;
        }
    }
}
