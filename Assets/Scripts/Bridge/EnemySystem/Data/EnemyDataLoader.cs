using System.Collections.Generic;
using UnityEngine;

public static class EnemyDataLoader
{
    public static List<EnemyAIData> LoadEnemyAIList(string category)
    {
        string path = $"Data/Battlers/Enemies/{category}";
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>(path);

        var list = new List<EnemyAIData>();

        foreach (var file in jsonFiles)
        {
            EnemyAIData data = JsonUtility.FromJson<EnemyAIData>(file.text);
            list.Add(data);
        }

        return list;
    }
}
