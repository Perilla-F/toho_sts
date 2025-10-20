using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCoordinator : MonoBehaviour
{
    public static GameCoordinator Instance { get; private set; }

    public MapManager MapManager { get; private set; }
    public EventRunner EventRunner { get; private set; }
    [SerializeField] private SaveManager saveManager;
    private MapManager mapManager;
    private MapGenerator mapGenerator;
    private EventRunner eventRunner;

    public SaveManager Save => saveManager;


    [Header("Bridge / UI")]
    [SerializeField] private EventBridge eventBridge;
    [SerializeField] private BattleTransition battleTransition;

    private Dictionary<Vector2Int, Cell> _currentCells;

    private GameManager GameManager;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // シーンロードイベントを監視
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameManager = GameManager.Instance;
        mapManager = FindObjectOfType<MapManager>();
        eventRunner = FindObjectOfType<EventRunner>();
        mapGenerator = FindObjectOfType<MapGenerator>();

        mapManager.OnAutoSaveRequested += HandleAutoSaveRequested;

        eventRunner.OnOptionSelected += HandleEventOptionSelected;

    }

    private void Start()
    {
        if (saveManager.HasSaveData())
        {
            // セーブデータがある場合はロード
            var save = saveManager.LoadGame();
            if (save != null)
            {
                // System層に状態を復元（データ構築のみ）
                mapManager.SetMapState(save.Map.mapData,
                                       new Vector2Int(save.Map.cellX, save.Map.cellY),
                                       save.Map.lastEventData);

                // MapManagerがデータを再構築
                mapManager.GenerateMapData();
                return;
            }
        }

        // セーブがない場合は新規開始
        mapManager.GenerateMapData();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // MapSceneならMapManagerを探して初期化
        if (scene.name == "MapScene")
        {
            mapManager = FindObjectOfType<MapManager>();
            mapGenerator = FindObjectOfType<MapGenerator>();

            if (mapManager == null || mapGenerator == null)
                return;

            if (mapManager != null)
            {
                // 双方向イベントを仲介
                mapManager.OnMapLoadRequested += HandleMapLoadRequested;
                mapManager.OnMapGenerated += HandleMapGenerated;
                mapManager.OnAutoSaveRequested += HandleAutoSaveRequested;

                // MapManagerに初期マップ生成をリクエスト
                mapManager.GenerateMapData();
            }
        }
    }

    public void LoadGame()
    {
        SaveData data = saveManager.LoadGame();
        RestoreMap(data.Map);
    }

    #region Event制御

    public void StartEvent(MultiStepEvent evt)
    {
        // System層の EventRunner に伝える
        EventRunner.Instance.StartEvent(evt);
        // Bridge が自動で UI に通知する
    }

    #endregion

    #region 戦闘制御

    public void StartBattle(EncounterData encounter)
    {
        // マップ上の情報を BattleTransitionData にまとめる
        var data = new BattleTransitionData
        {
            Type = encounter.Type,
            EncounterId = encounter.EncounterID,
            CellX = MapManager.CurrentCell.GridPos.x,
            CellY = MapManager.CurrentCell.GridPos.y,
            CellStates = MapManager.GetMapCellStates()
        };

        // Battleシーンへ遷移（フェード込み）
        battleTransition.StartBattleTransition("BattleScene", data);
    }

    public void ReturnToMap()
    {
        // 戦闘終了後、マップシーンに戻す
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
    }

    #endregion

    private void HandleMapGenerated(Dictionary<Vector2Int, MapCellState> mapData)
    {
        mapGenerator.BuildUpUI(mapData);
    }

    private void HandleMapLoadRequested(Dictionary<Vector2Int, MapCellState> mapData)
    {
        mapGenerator.BuildUpUI(mapData);

        // foreach (var kv in mapData)
        // {
        //     kv.Value.Cell.OnClick = () => OnCellClicked(kv.Value);
        // }
    }

    private void HandleAutoSaveRequested()
    {
        saveManager.SaveGame();
    }

    private void OnCellClicked(Cell cell)
    {
        MapManager.SelectCell(cell);
        cell.Behavior?.OnPlayerEnter();
    }

    /// <summary>
    /// 新規マップ生成
    /// </summary>
    public void StartNewMap()
    {
        MapManager.Instance.GenerateMapData();

        var startCell = MapManager.Instance.GetCellAt(Vector2Int.zero);
        MapManager.Instance.SelectCell(startCell);
    }

    /// <summary>
    /// セーブデータから復元
    /// </summary>
    public void RestoreMap(MapSaveData saveData)
    {
        MapGenerator.Instance.BuildUpUI(saveData.mapData);

        MapManager.Instance.SetMapState(
            saveData.mapData,
            new Vector2Int(saveData.cellX, saveData.cellY),
            saveData.lastEventData
        );

        var cell = MapManager.Instance.GetCellAt(new Vector2Int(saveData.cellX, saveData.cellY));
        MapManager.Instance.SelectCell(cell);

        // 未完了イベントがあれば再開
        if (MapManager.Instance.LastEventData != null && !MapManager.Instance.LastEventData.isCompleted)
        {
            var evt = EventDatabase.Instance.GetEvent(MapManager.Instance.LastEventData.eventId) as MultiStepEvent;
            EventRunner.Instance.StartEvent(evt);
        }
    }

    /// <summary>
    /// イベント終了時に呼ばれる
    /// MapManagerに状態更新を依頼し、SaveManagerで自動セーブ
    /// </summary>
    public void OnEventCompleted(string eventId, string selectedOptionId)
    {
        MapManager.CompleteLastEvent(selectedOptionId);
        saveManager.SaveGame();
    }

    private void HandleEventOptionSelected(EventOption option)
    {
        // セーブやマップ更新、フラグ管理など
    }

}