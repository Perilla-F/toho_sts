using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<EnemyUnit> enemies = new List<EnemyUnit>();
    public List<EnemyUnit> Enemies { get => enemies; }

    public void RegisterEnemy(EnemyUnit enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyUnit enemy)
    {
        enemies.Remove(enemy);
    }

    public EnemyUnit GetRandomAliveEnemy()
    {
        var aliveEnemies = enemies.FindAll(e => e != null && e.IsAlive());
        if (aliveEnemies.Count == 0) return null;
        return aliveEnemies[Random.Range(0, aliveEnemies.Count)];
    }

    public bool AreAllEnemiesDefeated()
    {
        return enemies.TrueForAll(e => !e.IsAlive());
    }

    public List<EnemyUnit> GetAllEnemies()
    {
        return enemies;
    }

    public void ClearEnemies()
    {
        enemies.Clear();
    }
}
