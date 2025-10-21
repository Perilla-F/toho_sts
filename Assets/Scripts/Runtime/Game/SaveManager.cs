using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                var prefab = Resources.Load<SaveManager>("Prefabs/Managers/SaveManager");
                if (prefab != null)
                {
                    Instantiate(prefab);
                }
                else
                {
                    Debug.LogError("SaveManager prefab not found in Resources!");
                }
            }
            return instance;
        }
    }

    private string savePath;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void SaveGame()
    {
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

        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("セーブ完了: " + savePath);

        Debug.Log("Game saved to: " + savePath);
    }

    public SaveData LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found!");
            return null;
        }

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
    }

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
