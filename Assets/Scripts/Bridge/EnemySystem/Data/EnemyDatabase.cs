using System.Collections.Generic;
using UnityEngine;

public static class EnemyDatabase
{
    private static Dictionary<string, EnemyData> _enemyDataMap;

    // 初期化（最初の呼び出し時に自動的にロード）
    private static void EnsureInitialized()
    {
        if (_enemyDataMap != null) return;

        _enemyDataMap = new Dictionary<string, EnemyData>();

        // Resources/EnemyAssets フォルダ内の EnemyData をすべて読み込む
        var allEnemyData = Resources.LoadAll<EnemyData>("EnemyAssets");

        foreach (var data in allEnemyData)
        {
            if (!_enemyDataMap.ContainsKey(data.EnemyId))
            {
                _enemyDataMap.Add(data.EnemyId, data);
            }
            else
            {
                Debug.LogWarning($"Duplicate enemyId detected: {data.EnemyId}");
            }
        }
    }
    /// <summary>
    /// IDから EnemyData を取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static EnemyData GetEnemyDataById(string id)
    {
        EnsureInitialized();

        if (_enemyDataMap.TryGetValue(id, out var data))
        {
            return data;
        }
        else
        {
            Debug.LogError($"EnemyData with ID '{id}' not found.");
            return null;
        }
    }

    // 全データ取得（必要なら）
    public static IEnumerable<EnemyData> GetAll()
    {
        EnsureInitialized();
        return _enemyDataMap.Values;
    }
}
