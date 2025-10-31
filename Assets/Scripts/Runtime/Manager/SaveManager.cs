using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour, ISaveService
{
    private const string GameKey = "GameSaveData";

    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

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
