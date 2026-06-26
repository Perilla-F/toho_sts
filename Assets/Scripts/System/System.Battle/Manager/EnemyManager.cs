using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : IEnemyManager
{
    private Dictionary<int, IEnemyUnit> _enemyDatabase = new();

    public IEnemyUnit GetEnemy(int id)
    {
        return _enemyDatabase.TryGetValue(id, out var enemy) ? enemy : null;
    }

    /// <summary>
    /// 生存エネミーのリストに登録
    /// </summary>
    /// <param name="enemy"></param>
    public void RegisterEnemy(int id, IEnemyUnit enemy)
    {
        _enemyDatabase[id] = enemy;
    }

    /// <summary>
    /// 生存エネミーのリストから除去
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemy(int id)
    {
        _enemyDatabase.Remove(id);
    }

    /// <summary>
    /// 生存エネミーからランダムに選択
    /// </summary>
    /// <returns></returns>
    private IEnemyUnit GetRandomAliveEnemy()
    {
        var aliveEnemies = GetAllEnemies().FindAll(e => e != null && e.IsAlive());
        if (aliveEnemies.Count == 0) return null;
        return aliveEnemies[Random.Range(0, aliveEnemies.Count)];
    }
    IEnemyUnit IEnemyManager.GetRandomAliveEnemy() => GetRandomAliveEnemy();
    IReadOnlyEnemyUnit IReadOnlyEnemyManager.GetRandomAliveEnemy() => GetRandomAliveEnemy();

    /// <summary>
    /// 敵の全滅確認
    /// </summary>
    /// <returns></returns>
    public bool AreAllEnemiesDefeated()
    {
        return GetAllEnemies().TrueForAll(e => !e.IsAlive());
    }

    /// <summary>
    /// 生存エネミーのリストを返す
    /// </summary>
    /// <returns></returns>
    public List<IEnemyUnit> GetAllEnemies()
    {
        return _enemyDatabase.Values.ToList();
    }
    List<IEnemyUnit> IEnemyManager.GetAllEnemies() => GetAllEnemies();
    IReadOnlyList<IReadOnlyEnemyUnit> IReadOnlyEnemyManager.GetAllEnemies() => GetAllEnemies();

    /// <summary>
    /// 生存エネミーをすべて除去
    /// </summary>
    public void ClearEnemies()
    {
        _enemyDatabase.Clear();
    }

}
