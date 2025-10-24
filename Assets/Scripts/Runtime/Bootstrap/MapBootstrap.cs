using System.Collections.Generic;
using UnityEngine;

public class MapBootstrap : MonoBehaviour
{
    private GameManager gameManager;
    private MapManager mapManager;
    private EventManager eventManager;
    private FlagManager flagManager;
    private SaveManager saveManager;

    [SerializeField] private EventUIManager eventUIManager;

    [Header("Map Settings")]
    [SerializeField] private MapGenerationRule rules;

    [Header("Event Data")]
    [SerializeField] private EventDatabase EventDatabase;

    private GameContext gameContext;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        mapManager = ServiceLocator.Get<MapManager>();
        eventManager = ServiceLocator.Get<EventManager>();
        flagManager = ServiceLocator.Get<FlagManager>();
        saveManager = ServiceLocator.Get<SaveManager>();

        ServiceLocator.Register(eventUIManager);

        gameContext = gameManager.GetGameContext();

        if (saveManager.HasSaveData())
        {
            // セーブデータがある場合はロード
            var save = saveManager.LoadGame();
            if (save != null)
            {
                // System層に状態を復元（データ構築のみ）
                mapManager.SetMapState(
                    save.Map.mapData,
                    new Vector2Int(save.Map.cellX, save.Map.cellY)
                    );

                // MapManagerがデータを再構築
                mapManager.RestoreMap(save);
                return;
            }
        }

        // MapManagerに初期マップ生成をリクエスト
        mapManager.GenerateMap();
    }
    private void Start()
    {
        ServiceLocator.Get<GameBootstrap>().OnEventUIManagerReady(eventUIManager);
    }

    private void HandleAutoSaveRequested()
    {
        SaveMap();
    }

    private void HandleEventOptionSelected(EventOption option)
    {
        // セーブやマップ更新、フラグ管理など
    }

    public void SaveMap()
    {
        var saveData = mapManager.CreateSaveData();
        saveManager.SaveMap(saveData);
    }

}
