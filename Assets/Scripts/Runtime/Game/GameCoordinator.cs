using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCoordinator : MonoBehaviour
{
    public static GameCoordinator instance;
    public static GameCoordinator Instance
    {
        get
        {
            if (instance == null)
            {
                var prefab = Resources.Load<GameCoordinator>("Prefabs/Managers/GameCoordinator");
                if (prefab != null)
                {
                    Instantiate(prefab);
                }
                else
                {
                    Debug.LogError("GameCoordinator prefab not found in Resources!");
                }
            }
            return instance;
        }
    }

    public MapManager MapManager { get; private set; }
    public EventRunner EventRunner { get; private set; }
    private SaveManager saveManager;

    public SaveManager Save => saveManager;


    [Header("Bridge / UI")]
    [SerializeField] private EventBridge eventBridge;
    [SerializeField] private BattleTransition battleTransition;

    private Dictionary<Vector2Int, Cell> _currentCells;

    private GameManager GameManager;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // シーンロードイベントを監視
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameManager = GameManager.Instance;
        saveManager = FindObjectOfType<SaveManager>();

    }

    private void Start()
    {
    }

    public void InitializeGame()
    {
        Debug.Log("GameCoordinator initialized!");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }

    public void LoadGame()
    {
        SaveData data = saveManager.LoadGame();
    }

    #region Event制御

    #endregion

    #region 戦闘制御

    public void StartBattle(EncounterData encounter)
    {
        // マップ上の情報を BattleTransitionData にまとめる
        var data = new BattleTransitionData
        {
            Type = encounter.Type,
            EncounterId = encounter.EncounterID,
            CellX = MapManager.CurrentCell.x,
            CellY = MapManager.CurrentCell.y,
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

}