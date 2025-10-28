using System.Collections.Generic;
using UnityEngine;

public class MapBootstrap : MonoBehaviour
{

    [Header("Managers")]
    private GameManager gameManager;
    private SaveManager saveManager;
    private MapManager mapManager;
    private EventManager eventManager;

    private MapPresenter mapPresenter;
    private EventPresenter eventPresenter;

    [Header("UI")]
    [SerializeField] private MapView mapView;
    [SerializeField] private EventUIManager eventUIManager;

    private void Awake()
    {
        saveManager = ServiceLocator.Get<SaveManager>();
        mapPresenter = new MapPresenter(gameManager, mapManager, eventManager, mapView);
        eventPresenter = new EventPresenter(eventManager, eventUIManager);

        if (saveManager.HasSaveData())
        {
            // セーブデータがある場合はロード
            var save = saveManager.LoadGame();
            if (save != null)
            {
                // MapManagerがデータを再構築
                mapManager.RestoreFrom(save.Map);
                return;
            }
        }

        // MapManagerに初期マップ生成をリクエスト
        mapManager.GenerateMap();
    }

}
