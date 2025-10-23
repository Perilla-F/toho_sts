using System.Collections.Generic;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private string nextScene = "TitleScene";
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private AudioManager audioManager;

    private MapManager _mapManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // 共通サービス登録
        ServiceLocator.Register(this);
        ServiceLocator.Register(gameManager);
        ServiceLocator.Register(saveManager);
        ServiceLocator.Register(audioManager);
        ServiceLocator.Register(sceneLoader);

        RuntimeInstaller.InstallAll();

        // 最初のシーンをロード（TitleSceneなど）
        sceneLoader.LoadSceneAsync(nextScene);
    }

    public void OnMapManagerReady(MapManager mapManager)
    {
        _mapManager = mapManager;
    }

    public void SaveGame()
    {
        var mapData = _mapManager.CreateSaveData();
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

        _mapManager.LoadFromData(gameData.Map);
        gameManager.LoadFromData(gameData.Player);
    }

}
