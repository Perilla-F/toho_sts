using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private List<EnemyData> _enemyDataList;
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] Transform EnemyArea;
    [SerializeField] EnemyDatabase EnemyDB;
    [SerializeField] EnemyManager EnemyManager;
    [SerializeField] GameManager GameManager;


    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="spawnCount">敵の数</param>
    public void SpawnEnemies(string category, HeroUnit Hero)
    {

        List<EncounterData> bosses = EncounterLoader.LoadEncounters(category);
        var selectedEncounter = bosses[Random.Range(0, bosses.Count)];

        foreach (var id in selectedEncounter.EnemyIds)
        {
            GameObject enemyObj = Instantiate(_enemyPrefab, EnemyArea);
            EnemyUnit enemyUnit = enemyObj.GetComponent<EnemyUnit>();
            EnemyViewer enemyView = new EnemyViewer();
            EnemyData enemyData = EnemyDB.GetEnemyById(id);
            enemyView.transform.localPosition = GetEnemyPosition(1); // 適切に配置
            enemyUnit.Setup(enemyData);
            EnemyManager.RegisterEnemy(enemyUnit);
        }
    }

    /// <summary>
    /// 敵を横並び
    /// </summary>
    /// <param name="index">敵の番号</param>
    /// <returns>index番目の敵の位置Vertor3</returns>
    Vector3 GetEnemyPosition(int index)
    {
        float spacing = 2.5f;
        Vector3 basePosition = new Vector3(-spacing, 0, 0);
        return basePosition + new Vector3(index * spacing, 0, 0);
    }

}
