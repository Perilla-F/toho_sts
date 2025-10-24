using System.Collections.Generic;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private string nextScene = "TitleScene";
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private FlagManager flagManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // 共通サービス登録
        ServiceLocator.Register(this);
        ServiceLocator.Register(gameManager);
        ServiceLocator.Register(saveManager);
        ServiceLocator.Register(audioManager);
        ServiceLocator.Register(sceneLoader);
        ServiceLocator.Register(mapManager);
        ServiceLocator.Register(eventManager);
        ServiceLocator.Register(flagManager);

        RuntimeInstaller.InstallAll();

        // 最初のシーンをロード（TitleSceneなど）
        sceneLoader.LoadSceneAsync(nextScene);
    }

    public void SaveGame(MapSaveData mapData)
    {
        var playerData = gameManager.CreateSaveData();

        var gameData = new SaveData
        {
            Map = mapData,
            Player = playerData,
        };

        var saveManager = ServiceLocator.Get<ISaveManager>();
        saveManager.SaveGame(gameData);
    }

    public void LoadGame()
    {
        var saveManager = ServiceLocator.Get<ISaveManager>();
        var gameData = saveManager.LoadGame();
        if (gameData == null) return;

        gameManager.LoadFromData(gameData.Player);
    }

    public void OnEventUIManagerReady(EventUIManager uIManager)
    {
        ServiceLocator.Register(uIManager);
    }

}
