using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    private Dictionary<string, EnemyData> _enemyDict;

    [SerializeField] private string resourcesPath = "Data/Battlers/Enemies";

    private void BuildDictionary()
    {
        if (_enemyDict != null) return;

        _enemyDict = new Dictionary<string, EnemyData>();
        EnemyData[] all = Resources.LoadAll<EnemyData>(resourcesPath);

        foreach (var enemy in all)
        {
            if (enemy == null || string.IsNullOrEmpty(enemy.EnemyId))
            {
                Debug.LogWarning($"EnemyDataのEnemyIdが未設定: {enemy?.name}");
                continue;
            }

            if (_enemyDict.ContainsKey(enemy.EnemyId))
            {
                Debug.LogWarning($"EnemyId重複: {enemy.EnemyId}");
                continue;
            }

            _enemyDict.Add(enemy.EnemyId, enemy);
        }

        Debug.Log($"EnemyDatabase 初期化完了: {_enemyDict.Count} 体登録");
    }

    /// <summary>
    /// IDからEnemyDataを取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EnemyData GetEnemyById(string id)
    {
        BuildDictionary();
        return _enemyDict.TryGetValue(id, out var data) ? data : null;
    }
}