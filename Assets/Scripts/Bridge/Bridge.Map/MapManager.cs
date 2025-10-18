using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private EventDatabase eventDatabase;

    /// <summary>
    /// 現在のマップID
    /// </summary>
    /// <value></value>
    public string CurrentMapId { get; private set; } = "Map_1";

    /// <summary>
    /// 現在位置
    /// </summary>
    /// <value></value>
    public Vector2Int CurrentCellPos { get; private set; }

    /// <summary>
    /// 最後に発生したイベントID
    /// </summary>
    /// <value></value>
    public string LastEventId { get; private set; }
    public LastEventData LastEventData { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (var cell in FindObjectsOfType<BattleCell>())
        {
            // DI: Battle開始リクエストが来たらScene遷移
            cell.OnBattleRequest = encounter => StartCoroutine(StartBattleScene(encounter));
        }
    }

    private IEnumerator StartBattleScene(EncounterData encounter)
    {
        GameManager.Instance.CurrentEncounter = encounter;

        var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("BattleScene");
        while (!op.isDone) yield return null;

        // 遷移後に BattleStarter が Awake して Encounterを読む
    }

    public void SetLastEvent(string eventId)
    {
        LastEventData = new LastEventData
        {
            eventId = eventId,
            isCompleted = false,
            selectedOptionId = null
        };
    }

    public void CompleteLastEvent(string optionId)
    {
        if (LastEventData == null) return;

        LastEventData.isCompleted = true;
        LastEventData.selectedOptionId = optionId;

        SaveManager.Instance.SaveGame();
    }

    public void TryResumeLastEvent()
    {
        if (LastEventData != null && !LastEventData.isCompleted)
        {
            var evt = eventDatabase.GetEvent(LastEventData.eventId) as MultiStepEvent;
            EventManager.Instance.StartEvent(evt);
        }
    }

    private string GetNextMapId()
    {
        // 例: Map_1 → Map_2 に進む
        string[] parts = CurrentMapId.Split('_');
        int num = int.Parse(parts[1]);
        return $"Map_{num + 1}";
    }

    // セーブ用データ構築
    public MapSaveData GetSaveData()
    {
        return new MapSaveData
        {
            mapId = CurrentMapId,
            cellX = CurrentCellPos.x,
            cellY = CurrentCellPos.y,
            lastEventId = LastEventId
        };
    }

    // ロード時に復元
    public void LoadFromSaveData(MapSaveData data)
    {
        CurrentMapId = data.mapId;
        CurrentCellPos = new Vector2Int(data.cellX, data.cellY);
        LastEventId = data.lastEventId;

        mapGenerator.GenerateMap();
        // セル再選択
        var cell = mapGenerator.GetCellAt(CurrentCellPos);
        mapGenerator.SelectCell(cell);
    }

}