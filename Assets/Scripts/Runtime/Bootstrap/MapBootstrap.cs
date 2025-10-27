using System.Collections.Generic;
using UnityEngine;

public class MapBootstrap : MonoBehaviour
{
    [Header("MapRule")]
    [SerializeField] private MapGenerationRule generationRule;

    [Header("EventData")]
    [SerializeField] private EventDatabase eventDatabase;

    [Header("Managers")]
    private GameManager gameManager;
    private FlagManager flagManager;
    private SaveManager saveManager;
    private MapManager mapManager;
    private EventManager eventManager;

    private MapPresenter mapPresenter;
    private EventPresenter eventPresenter;

    [Header("UI")]
    [SerializeField] private MapView mapView;
    [SerializeField] private EventUIManager eventUIManager;

    [Header("Map Settings")]
    [SerializeField] private MapGenerationRule rules;

    [Header("Event Data")]
    [SerializeField] private EventDatabase EventDatabase;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        saveManager = ServiceLocator.Get<SaveManager>();

        mapManager = new MapManager(gameManager);
        flagManager = new FlagManager();
        eventManager = new EventManager(eventDatabase, gameManager, flagManager);

        mapPresenter = new MapPresenter(gameManager, mapManager, eventManager, mapView);
        eventPresenter = new EventPresenter(eventManager, eventUIManager);

        if (saveManager.HasSaveData())
        {
            // セーブデータがある場合はロード
            var save = saveManager.LoadGame();
            if (save != null)
            {
                // System層に状態を復元（データ構築のみ）
                mapView.BuildMapUI(save.Map.mapData, generationRule);

                // MapManagerがデータを再構築
                mapManager.RestoreMap(save, generationRule);
                return;
            }
        }

        // MapManagerに初期マップ生成をリクエスト
        mapManager.GenerateMap(generationRule);
    }

}
