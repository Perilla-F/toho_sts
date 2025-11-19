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
        ServiceLocator.Register<ISaveScheduler>(scheduler);

        // コンテキスト作成
        var context = new GameContext();
        ServiceLocator.Register<GameContext>(context);

        // ゲームマネージャー作成
        _gameManager = new GameManager(loader, save, scheduler, context);
        ServiceLocator.Register<GameManager>(_gameManager);

        // PlayerManager作成
        var playerManager = new PlayerManager(_gameManager);
        ServiceLocator.Register<PlayerManager>(playerManager);

        // 最初のシーンをロード（TitleSceneなど）
        ServiceLocator.Get<ISceneLoader>().LoadScene(nextScene);

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
