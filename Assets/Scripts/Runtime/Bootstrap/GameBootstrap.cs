using System.Collections.Generic;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [Header("FirstSceneName")]
    [SerializeField] private string nextScene = "TitleScene";

    [Header("ManagerPrefabs")]
    [SerializeField] private SaveManager saveManagerPrefab;
    [SerializeField] private AudioManager audioManagerPrefab;
    [SerializeField] private SceneLoader sceneLoaderPrefab;

    [Header("SystemPrefabs")]
    [SerializeField] private SaveScheduler saveSchedulerPrefab;

    [Header("MapRule")]
    [SerializeField] private MapGenerationRule generationRule;

    [Header("EventData")]
    [SerializeField] private EventDatabase eventDatabase;

    public GameManager _gameManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // 共通サービス登録
        ServiceLocator.Register(this);

        var save = Instantiate(saveManagerPrefab);
        var audio = Instantiate(audioManagerPrefab);
        var loader = Instantiate(sceneLoaderPrefab);
        var scheduler = Instantiate(saveSchedulerPrefab);

        DontDestroyOnLoad(save);
        DontDestroyOnLoad(audio);
        DontDestroyOnLoad(loader);
        DontDestroyOnLoad(scheduler);
        ServiceLocator.Register<ISaveService>(save);
        ServiceLocator.Register<IAudioManager>(audio);
        ServiceLocator.Register<ISceneLoader>(loader);

        // 各種Manager作成
        var playerManager = new PlayerManager(null);
        var mapManger = new MapManager(null, generationRule);
        var flagManager = new FlagManager();
        var eventManager = new EventManager(eventDatabase, null, flagManager);

        // コンテキスト作成
        var context = new GameContext(playerManager, mapManger, eventManager);

        // ゲームマネージャー作成
        _gameManager = new GameManager(loader, save, scheduler, context);

        playerManager.Inject(_gameManager);
        mapManger.Inject(_gameManager);
        eventManager.Inject(_gameManager);

        // 最初のシーンをロード（TitleSceneなど）
        ServiceLocator.Get<SceneLoader>().LoadSceneAsync(nextScene);

        // SaveManagerに購読登録（セーブイベントを受ける）
        save.Subscribe(_gameManager);
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.F5))
    //     {
    //         Debug.Log("Manual Save Triggered");

    //         // ダミーデータでテスト
    //         var map = new MapSaveData();
    //         var evt = new EventSaveData();
    //         _gameManager.RequestSave(map, evt, "Player001");
    //     }
    // }
}
