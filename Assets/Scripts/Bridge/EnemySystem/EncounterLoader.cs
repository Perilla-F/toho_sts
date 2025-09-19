using System.Collections.Generic;
using UnityEngine;

public static class EncounterLoader
{
    /// <summary>
    /// 参照カテゴリーの敵の名前とIDのリストを返す
    /// </summary>
    /// <param name="category">参照パス"Data/Battlers/Encounters/{category}"</param>
    /// <returns></returns>
    public static List<EncounterData> LoadEncounters(string category)
    {
        string path = $"Data/Battlers/Encounters/{category}";
        TextAsset[] files = Resources.LoadAll<TextAsset>(path);

        var list = new List<EncounterData>();
        foreach (var file in files)
        {
            list.Add(JsonUtility.FromJson<EncounterData>(file.text));
        }

        return list;
    }
}
