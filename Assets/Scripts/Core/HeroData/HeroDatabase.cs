using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HeroData/HeroDatabase")]
public class HeroDatabase : ScriptableObject
{
    [SerializeField] public List<HeroData> Datas;
    private Dictionary<string, HeroData> dict;

    public static HeroDatabase Instance;

    private void OnEnable()
    {
        Instance = this;
        dict = new Dictionary<string, HeroData>();
        foreach (var data in Datas)
        {
            dict[data.HeroID] = data;
        }
    }

    public HeroData GetHeroData(string id)
    {
        dict ??= new Dictionary<string, HeroData>();
        if (dict.TryGetValue(id, out var data))
            return data;

        Debug.LogWarning($"HeroData not found: {id}");
        return null;
    }

}