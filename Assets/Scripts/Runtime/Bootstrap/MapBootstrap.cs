using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEngine;

public class MapBootstrap : MonoBehaviour
{

    [Header("Managers")]
    private GameManager _gameManager;
    private ISaveService _saveManager;
    private MapManager _mapManager;
    private EventManager _eventManager;
    private FlagManager _flagManager;

    private GameContext _context;

    private MapPresenter mapPresenter;
    private EventPresenter eventPresenter;

    [Header("UI")]
    [SerializeField] private MapView _mapView;
    [SerializeField] private EventUIManager _eventUIManager;
    [SerializeField] private RestUIManager _restUIManager;

    [Header("Data")]
    [SerializeField] private MapGenerationRule _rule;
    [SerializeField] private EventDatabase _eventDatabase;

    private void Awake()
    {
        _gameManager = ServiceLocator.Get<GameManager>();
        _saveManager = ServiceLocator.Get<ISaveService>();
        _context = ServiceLocator.Get<GameContext>();

        _flagManager = new FlagManager();
        _eventManager = new EventManager(_eventDatabase, _gameManager, _flagManager, _context);
        _mapManager = new MapManager(_gameManager, _context, _rule);

        mapPresenter = new MapPresenter(_gameManager, _mapManager, _eventManager, _mapView);
        eventPresenter = new EventPresenter(_eventManager, _eventUIManager, _restUIManager);

        if (_context.MapData != null)
        {
            UnityEngine.Debug.Log("MapLoaded!");
            // MapManagerがデータを再構築
            _mapManager.RestoreFrom();
            return;
        }

        UnityEngine.Debug.Log("NewMapGenerate!");
        // MapManagerに初期マップ生成をリクエスト
        _mapManager.GenerateMap();
    }

}
