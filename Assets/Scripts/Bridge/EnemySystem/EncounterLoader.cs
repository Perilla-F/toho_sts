using System.Collections.Generic;
using UnityEngine;

public static class EncounterLoader
{
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
