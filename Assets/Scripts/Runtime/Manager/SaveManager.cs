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
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved!");
    }

    public SaveData LoadGame()
    {
        if (!File.Exists(savePath))
            return null;

        string json = File.ReadAllText(savePath);
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
