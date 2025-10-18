using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string savePath;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void SaveGame()
    {
        var data = new SaveData
        {
            LastEvent = MapManager.Instance.LastEventData,
            HPResource = GameManager.Instance.HeroBattler.HPResource,
            //gold = PlayerData.Instance.Gold,
            Flags = new List<string>(FlagManager.Instance.GetAllFlags()),
            Map = MapManager.Instance.GetSaveData()
        };

        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("セーブ完了: " + savePath);

        Debug.Log("Game saved to: " + savePath);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found!");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // --- 各マネージャーにデータを反映 ---
        GameManager.Instance.HeroBattler.HPResource = data.HPResource;
        /// PlayerData.Instance.Gold = data.gold;
        FlagManager.Instance.LoadFromSaveData(data.Flags);
        MapManager.Instance.LoadFromSaveData(data.Map);
        MapManager.Instance.LoadFromSaveData(data.Map);
        MapManager.Instance.LastEventData = data.LastEvent;

        MapManager.Instance.TryResumeLastEvent();

        Debug.Log("Game loaded!");
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}
