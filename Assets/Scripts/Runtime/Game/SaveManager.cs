using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour, ISaveManager
{
    private GameManager gameManager;
    private FlagManager flagManager;

    private const string MapKey = "MapSaveData";
    private const string GameKey = "GameSaveData";

    private string savePath;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        gameManager = ServiceLocator.Get<GameManager>();
        flagManager = ServiceLocator.Get<FlagManager>();
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    #region Map単体の保存/ロード
    public void SaveMap(MapSaveData data)
    {
<<<<<<< HEAD
        var data = new SaveData
        {
            LastEvent = EventBridge.Instance.runner.LastEventData,
            HPResource = GameManager.Instance.HeroBattler.HPResource,
            //gold = PlayerData.Instance.Gold,
            Flags = new List<string>(FlagManager.Instance.GetAllFlags()),
            Map = new MapSaveData
            {
                mapData = MapBootstrap.Instance.Manager.mapData,
                cellX = MapBootstrap.Instance.Manager.CurrentCell.x,
                cellY = MapBootstrap.Instance.Manager.CurrentCell.y,
                lastEventData = EventBridge.Instance.runner.LastEventData
            }
        };
=======
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(MapKey, json);
        PlayerPrefs.Save();
        Debug.Log("Map saved!");
    }
>>>>>>> origin/battle-system-laptop

    public MapSaveData LoadMap()
    {
        if (!PlayerPrefs.HasKey(MapKey))
            return null;

        string json = PlayerPrefs.GetString(MapKey);
        return JsonUtility.FromJson<MapSaveData>(json);
    }
    #endregion

    #region 統合セーブ
    public void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(GameKey, json);
        PlayerPrefs.Save();
        Debug.Log("Game saved!");
    }

    public SaveData LoadGame()
    {
        if (!PlayerPrefs.HasKey(GameKey))
            return null;

<<<<<<< HEAD
        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        return data;
        // --- 各マネージャーにデータを反映 ---
        GameManager.Instance.HeroBattler.HPResource = data.HPResource;
        /// PlayerData.Instance.Gold = data.gold;
        FlagManager.Instance.LoadFromSaveData(data.Flags);

        if (data.Map.lastEventData != null && !data.Map.lastEventData.isCompleted)
            EventBridge.Instance.runner.StartStep(data.LastEvent.stepId);

        Debug.Log("Game loaded!");
=======
        string json = PlayerPrefs.GetString(GameKey);
        return JsonUtility.FromJson<SaveData>(json);
>>>>>>> origin/battle-system-laptop
    }
    #endregion

    public bool HasSaveData()
    {
        return File.Exists(savePath);
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}
