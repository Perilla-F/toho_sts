using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : IEnemyManager
{
    private List<IEnemyUnit> _enemies;

    public EnemyManager()
    {
        _enemies = new List<IEnemyUnit>();
    }

    /// <summary>
    /// 生存エネミーのリストに登録
    /// </summary>
    /// <param name="enemy"></param>
    public void RegisterEnemy(IEnemyUnit enemy)
    {
        _enemies.Add(enemy);
    }

    /// <summary>
    /// 生存エネミーのリストから除去
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemy(IEnemyUnit enemy)
    {
        _enemies.Remove(enemy);
    }

    /// <summary>
    /// 生存エネミーからランダムに選択
    /// </summary>
    /// <returns></returns>
    private IEnemyUnit GetRandomAliveEnemy()
    {
        var aliveEnemies = _enemies.FindAll(e => e != null && e.IsAlive());
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
        return _enemies.TrueForAll(e => !e.IsAlive());
    }

    /// <summary>
    /// 生存エネミーのリストを返す
    /// </summary>
    /// <returns></returns>
    public List<IEnemyUnit> GetAllEnemies()
    {
        return _enemies;
    }
    List<IEnemyUnit> IEnemyManager.GetAllEnemies() => GetAllEnemies();
    IReadOnlyList<IReadOnlyEnemyUnit> IReadOnlyEnemyManager.GetAllEnemies() => GetAllEnemies();

    /// <summary>
    /// 生存エネミーをすべて除去
    /// </summary>
    public void ClearEnemies()
    {
        _enemies.Clear();
    }

}
