using System.Collections.Generic;
using UnityEngine;

public class MapBootstrap : MonoBehaviour
{
    public MapManager Manager { get; private set; }
    public EventRunner Runner { get; private set; }
    private GameManager gameManager;
    private FlagManager flagManager;
    private SaveManager saveManager;

    [SerializeField] private MapGenerator generator;
    [SerializeField] private EventUIManager eventUIManager;

    [Header("Map Settings")]
    [SerializeField] private int Width = 5;
    [SerializeField] private int Height = 10;

    [Header("Event Data")]
    [SerializeField] private EventDatabase EventDatabase;

    private GameContext gameContext;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        flagManager = ServiceLocator.Get<FlagManager>();
        saveManager = ServiceLocator.Get<SaveManager>();

        gameContext = gameManager.GetGameContext();

        Manager = new MapManager(generator, Width, Height);
        Runner = new EventRunner(gameContext, flagManager);

        if (Manager != null)
        {
            // 双方向イベントを仲介
            generator.OnCellClicked += HandleCellClicked;

            if (Runner != null)
            {
                Runner.OnOptionSelected += HandleEventOptionSelected;
            }
            if (saveManager.HasSaveData())
            {
                // セーブデータがある場合はロード
                var save = saveManager.LoadGame();
                if (save != null)
                {
                    // System層に状態を復元（データ構築のみ）
                    Manager.SetMapState(
                        save.Map.mapData,
                        new Vector2Int(save.Map.cellX, save.Map.cellY)
                        );

                    // MapManagerがデータを再構築
                    Manager.GenerateMapData(new(save.Map.cellX, save.Map.cellY));
                    return;
                }
            }

            // MapManagerに初期マップ生成をリクエスト
            Manager.GenerateMapData(new(Width / 2, 0));
        }
    }

    private void Start()
    {
        ServiceLocator.Get<GameEntryPoint>().OnMapManagerReady(Manager);
    }

    private void HandleAutoSaveRequested()
    {
        SaveMap();
    }

    private void HandleCellClicked(Vector2Int pos)
    {
        Manager.OnCellClicked(pos);
    }

    /// <summary>
    /// 新規マップ生成
    /// </summary>
    public void StartNewMap()
    {
        Manager.GenerateMapData(new(Width / 2, 0));
    }

    /// <summary>
    /// セーブデータから復元
    /// </summary>
    public void RestoreMap(MapSaveData saveData)
    {
        generator.BuildUpUI(saveData.mapData);

        Manager.SetMapState(
            saveData.mapData,
            new Vector2Int(saveData.cellX, saveData.cellY)
        );
    }

    private void HandleEventOptionSelected(EventOption option)
    {
        // セーブやマップ更新、フラグ管理など
    }

    public void StartEvent(MultiStepEvent evt)
    {
        Runner.StartEvent(evt);
    }

    public void StartRandomEvent()
    {
        Runner.StartEvent(EventDatabase.GetRandomEvent(gameContext, flagManager));
    }

    /// <summary>
    /// イベント再開
    /// </summary>
    public void TryResumeLastEvent(SaveData saveData)
    {
        if (saveData.Map.lastEventData != null && !saveData.Map.lastEventData.isCompleted)
        {
            var evt = EventDatabase.GetEvent(saveData.Map.lastEventData.eventId) as MultiStepEvent;
            Runner.StartStep(saveData.Map.lastEventData.stepId);
        }
    }

    public void SaveMap()
    {
        var saveData = Manager.CreateSaveData();
        var saveManager = ServiceLocator.Get<SaveManager>();
        saveManager.SaveMap(saveData);
    }

}
