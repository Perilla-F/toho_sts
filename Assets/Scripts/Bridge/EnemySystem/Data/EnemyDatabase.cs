using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabase
{
    private Dictionary<string, EnemyData> _enemyDict = new();

    public void LoadFromJson(string json)
    {
        var wrapper = JsonUtility.FromJson<EnemyDataList>(json);
        foreach (var enemy in wrapper.Enemies)
        {
            // エフェクトをインスタンス化したRuntime版に変換
            _enemyDict[enemy.EnemyId] = enemy;
        }
    }

    public EnemyData GetEnemyById(string id)
    {
        return _enemyDict.TryGetValue(id, out var enemy) ? enemy : null;
    }

}
