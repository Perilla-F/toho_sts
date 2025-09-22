using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<EnemyUnit> _enemies = new List<EnemyUnit>();
    public List<EnemyUnit> Enemies { get => _enemies; }

    /// <summary>
    /// 生存エネミーのリストに登録
    /// </summary>
    /// <param name="enemy"></param>
    public void RegisterEnemy(EnemyUnit enemy)
    {
        _enemies.Add(enemy);
    }

    /// <summary>
    /// 生存エネミーのリストから除去
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemy(EnemyUnit enemy)
    {
        _enemies.Remove(enemy);
    }

    /// <summary>
    /// 生存エネミーからランダムに選択
    /// </summary>
    /// <returns></returns>
    public EnemyUnit GetRandomAliveEnemy()
    {
        var aliveEnemies = _enemies.FindAll(e => e != null && e.IsAlive());
        if (aliveEnemies.Count == 0) return null;
        return aliveEnemies[Random.Range(0, aliveEnemies.Count)];
    }

    public bool AreAllEnemiesDefeated()
    {
        return _enemies.TrueForAll(e => !e.IsAlive());
    }

    /// <summary>
    /// 生存エネミーのリストを返す
    /// </summary>
    /// <returns></returns>
    public List<EnemyUnit> GetAllEnemies()
    {
        return _enemies;
    }

    /// <summary>
    /// 生存エネミーをすべて除去
    /// </summary>
    public void ClearEnemies()
    {
        _enemies.Clear();
    }
}
