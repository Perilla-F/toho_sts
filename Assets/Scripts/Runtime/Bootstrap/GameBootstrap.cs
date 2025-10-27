using System.Collections.Generic;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private string nextScene = "TitleScene";
    [SerializeField] private AudioManager audioManagerPrefab;
    [SerializeField] private SceneLoader sceneLoaderPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // 共通サービス登録
        ServiceLocator.Register(this);
        ServiceLocator.Register(new SaveManager());
        ServiceLocator.Register(new PlayerManager());

        var audio = Instantiate(audioManagerPrefab);
        var loader = Instantiate(sceneLoaderPrefab);

        ServiceLocator.Register<IAudioManager>(audio);
        ServiceLocator.Register<ISceneLoader>(loader);

        // 最初のシーンをロード（TitleSceneなど）
        ServiceLocator.Get<SceneLoader>().LoadSceneAsync(nextScene);

        GameManager gameManager = new GameManager(loader);
    }

    public void SaveGame(MapSaveData mapData)
    {
        var playerData = ServiceLocator.Get<PlayerManager>().CreateSaveData();

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

        ServiceLocator.Get<PlayerManager>().LoadFromData(gameData.Player);
    }

}
