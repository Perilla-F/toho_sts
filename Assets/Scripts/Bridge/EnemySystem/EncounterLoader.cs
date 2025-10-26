using System.Collections.Generic;
using UnityEngine;

public static class EncounterLoader
{
    /// <summary>
    /// 参照カテゴリーの敵グループ情報のリストを返す
    /// </summary>
    /// <param name="type">Normal or Elite or Boss</param>
    /// <param name="stageIndex">1 or 2 or 3</param>
    /// <returns></returns>
    public static List<EncounterData> LoadEncounters(EnemyType type, int stageIndex)
    {
        // Resources フォルダの相対パスを組み立て
        string category = $"{type}/Stage{stageIndex}";
        string path = $"Data/Battlers/Encounters/{category}";

        // 指定パス以下の EncounterData をすべてロード
        EncounterData[] assets = Resources.LoadAll<EncounterData>(path);

        // nullチェック + List化して返す
        if (assets == null || assets.Length == 0)
        {
            Debug.LogWarning($"EncounterLoader: 指定パス '{path}' に EncounterData が見つかりません。");
            return new List<EncounterData>();
        }

        return new List<EncounterData>(assets);
    }
}
