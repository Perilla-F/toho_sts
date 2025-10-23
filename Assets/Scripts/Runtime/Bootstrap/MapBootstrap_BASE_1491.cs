using System.Collections.Generic;
using UnityEngine;

public class MapBootstrap : MonoBehaviour
{
    public static MapBootstrap Instance { get; private set; }
    public MapManager Manager { get; private set; }
    public EventRunner Runner { get; private set; }

    [SerializeField] private MapGenerator generator;
    [SerializeField] private EventUIManager eventUIManager;

    [Header("Map Settings")]
    [SerializeField] private int Width = 5;
    [SerializeField] private int Height = 10;

    [Header("Event Data")]
    [SerializeField] private EventDatabase EventDatabase;

    private void Awake()
    {
        Instance = this;
        Manager = new MapManager(generator, Width, Height);
        Runner = new EventRunner();

        if (Manager != null)
        {
            // 双方向イベントを仲介
            generator.OnCellClicked += HandleCellClicked;

            if (Runner != null)
            {
                Runner.OnOptionSelected += HandleEventOptionSelected;
            }
            if (SaveManager.Instance.HasSaveData())
            {
                // セーブデータがある場合はロード
                var save = SaveManager.Instance.LoadGame();
                if (save != null)
                {
                    // System層に状態を復元（データ構築のみ）
                    Manager.SetMapState(
                        save.Map.mapData,
                        new Vector2Int(save.Map.cellX, save.Map.cellY),
                        save.Map.lastEventData
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

    private void HandleMapGenerated(Dictionary<Vector2Int, MapCellState> mapData)
    {
        generator.BuildUpUI(mapData);
    }

    private void HandleMapLoadRequested(Dictionary<Vector2Int, MapCellState> mapData)
    {
        generator.BuildUpUI(mapData);
    }

    private void HandleAutoSaveRequested()
    {
        SaveManager.Instance.SaveGame();
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
        MapGenerator.Instance.BuildUpUI(saveData.mapData);

        Manager.SetMapState(
            saveData.mapData,
            new Vector2Int(saveData.cellX, saveData.cellY),
            saveData.lastEventData
        );

        Manager.OnCellClicked(new Vector2Int(saveData.cellX, saveData.cellY));

        // 未完了イベントがあれば再開
        if (Manager.LastEventData != null && !Manager.LastEventData.isCompleted)
        {
            var evt = EventDatabase.Instance.GetEvent(Manager.LastEventData.eventId) as MultiStepEvent;
            Runner.StartEvent(evt);
        }
    }

    /// <summary>
    /// イベント終了時に呼ばれる
    /// MapManagerに状態更新を依頼し、SaveManagerで自動セーブ
    /// </summary>
    public void OnEventCompleted(string eventId, string selectedOptionId)
    {
        Manager.CompleteLastEvent();
        SaveManager.Instance.SaveGame();
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
        Runner.StartEvent(EventDatabase.GetRandomEvent());
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

}
