using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : ISaveManager
{
    private GameManager gameManager;
    private FlagManager flagManager;

    private const string GameKey = "GameSaveData";
    private const string MapKey = "MapSaveData";
    private const string EventKey = "EventSaveData";

    private string savePath;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        flagManager = ServiceLocator.Get<FlagManager>();
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    #region Map単体の保存/ロード
    public void SaveMap(MapSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(MapKey, json);
        PlayerPrefs.Save();
        Debug.Log("Map saved!");
    }

    public MapSaveData LoadMap()
    {
        if (!PlayerPrefs.HasKey(MapKey))
            return null;

        string json = PlayerPrefs.GetString(MapKey);
        return JsonUtility.FromJson<MapSaveData>(json);
    }
    #endregion

    #region Event単体の保存/ロード
    public void SaveEvent(EventSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(EventKey, json);
        PlayerPrefs.Save();
        Debug.Log("Event saved!");
    }

    public EventSaveData LoadEvent()
    {
        if (!PlayerPrefs.HasKey(EventKey))
            return null;

        string json = PlayerPrefs.GetString(EventKey);
        return JsonUtility.FromJson<EventSaveData>(json);
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

        string json = PlayerPrefs.GetString(GameKey);
        return JsonUtility.FromJson<SaveData>(json);
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
