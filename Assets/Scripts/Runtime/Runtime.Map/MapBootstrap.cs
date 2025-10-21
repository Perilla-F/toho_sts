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
        Runner = new EventRunner(eventUIManager);

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
        Manager.SetMapState(
            saveData.mapData,
            new Vector2Int(saveData.cellX, saveData.cellY)
        );
    }

    private void HandleEventOptionSelected(EventOption option)
    {
        // セーブやマップ更新、フラグ管理など
    }

}
