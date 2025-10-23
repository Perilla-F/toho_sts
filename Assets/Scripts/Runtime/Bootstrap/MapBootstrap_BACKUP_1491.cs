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
<<<<<<< HEAD:Assets/Scripts/Runtime/Runtime.Map/MapBootstrap.cs
        Runner = new EventRunner(eventUIManager);
=======
        Runner = new EventRunner(gameContext, flagManager);
>>>>>>> origin/battle-system-laptop:Assets/Scripts/Runtime/Bootstrap/MapBootstrap.cs

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

<<<<<<< HEAD:Assets/Scripts/Runtime/Runtime.Map/MapBootstrap.cs
=======
    private void Start()
    {
        ServiceLocator.Get<GameEntryPoint>().OnMapManagerReady(Manager);
    }

>>>>>>> origin/battle-system-laptop:Assets/Scripts/Runtime/Bootstrap/MapBootstrap.cs
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
<<<<<<< HEAD:Assets/Scripts/Runtime/Runtime.Map/MapBootstrap.cs
=======
        generator.BuildUpUI(saveData.mapData);

>>>>>>> origin/battle-system-laptop:Assets/Scripts/Runtime/Bootstrap/MapBootstrap.cs
        Manager.SetMapState(
            saveData.mapData,
            new Vector2Int(saveData.cellX, saveData.cellY)
        );
<<<<<<< HEAD:Assets/Scripts/Runtime/Runtime.Map/MapBootstrap.cs
=======

        Manager.OnCellClicked(new Vector2Int(saveData.cellX, saveData.cellY));

        // 未完了イベントがあれば再開
        if (Manager.LastEventData != null && !Manager.LastEventData.isCompleted)
        {
            var evt = EventDatabase.GetEvent(Manager.LastEventData.eventId) as MultiStepEvent;
            Runner.StartEvent(evt);
        }
    }

    /// <summary>
    /// イベント終了時に呼ばれる
    /// MapManagerに状態更新を依頼し、SaveManagerで自動セーブ
    /// </summary>
    public void OnEventCompleted()
    {
        Manager.CompleteLastEvent();
        SaveMap();
>>>>>>> origin/battle-system-laptop:Assets/Scripts/Runtime/Bootstrap/MapBootstrap.cs
    }

    private void HandleEventOptionSelected(EventOption option)
    {
        // セーブやマップ更新、フラグ管理など
    }

<<<<<<< HEAD:Assets/Scripts/Runtime/Runtime.Map/MapBootstrap.cs
=======
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

>>>>>>> origin/battle-system-laptop:Assets/Scripts/Runtime/Bootstrap/MapBootstrap.cs
}
