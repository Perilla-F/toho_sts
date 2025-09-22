using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] Transform EnemyArea;
    [SerializeField] EnemyDatabase EnemyDB;
    [SerializeField] EnemyManager EnemyManager;
    [SerializeField] private Transform _enemyArea;        // Canvas内
    [SerializeField] private Transform _enemyModelsArea;  // モデル配置用
    [SerializeField] private float _modelBaseY = -200f;  // モデルのY初期位置


    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="category">参照パス"Data/Battlers/Encounters/{category}"</param>
    public void SpawnEnemies(string category)
    {

        List<EncounterData> bosses = EncounterLoader.LoadEncounters(category);
        var selectedEncounter = bosses[Random.Range(0, bosses.Count)];

        foreach (var id in selectedEncounter.EnemyIds)
        {
            EnemyUnit enemyUnit = new EnemyUnit();
            EnemyData enemyData = EnemyDB.GetEnemyById(id);
            enemyUnit.Setup(enemyData);

            // UI生成
            EnemyUI enemyUI = Instantiate(enemyUnit.UIPrefab, _enemyArea).GetComponent<EnemyUI>();
            enemyUI.Init(enemyUnit);

            // モデル生成
            EnemyModel enemyModel = Instantiate(enemyUnit.ModelPrefab, _enemyModelsArea).GetComponent<EnemyModel>();
            enemyModel.Init(enemyUnit);

            enemyUnit.Model = enemyModel; // バインド

            // モデルの初期位置をUIに合わせる
            Vector3 uiPos = enemyUI.transform.position;
            enemyModel.transform.position = new Vector3(uiPos.x, _modelBaseY + enemyUnit.ModelYOffset, 0);

            EnemyManager.RegisterEnemy(enemyUnit);
        }
    }

}
