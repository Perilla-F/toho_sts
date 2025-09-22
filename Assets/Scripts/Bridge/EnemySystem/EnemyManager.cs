using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<EnemyUnit> _enemies = new List<EnemyUnit>();
    public List<EnemyUnit> Enemies { get => _enemies; }

    public void RegisterEnemy(EnemyUnit enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyUnit enemy)
    {
        _enemies.Remove(enemy);
    }

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

    public List<EnemyUnit> GetAllEnemies()
    {
        return _enemies;
    }

    public void ClearEnemies()
    {
        _enemies.Clear();
    }
}
