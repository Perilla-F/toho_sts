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
        _mapManager = new MapManager(_gameManager, _rule);

        mapPresenter = new MapPresenter(_gameManager, _mapManager, _eventManager, _mapView);
        eventPresenter = new EventPresenter(_eventManager, _eventUIManager);

        _context.InjectMap(_mapManager);
        _context.InjectEvent(_eventManager);

        if (_saveManager.HasSaveData())
        {
            // セーブデータがある場合はロード
            var save = _saveManager.LoadGame();
            if (save != null)
            {
                // MapManagerがデータを再構築
                _mapManager.RestoreFrom(save.Map);
                return;
            }
        }

        // MapManagerに初期マップ生成をリクエスト
        _mapManager.GenerateMap();
    }

}
