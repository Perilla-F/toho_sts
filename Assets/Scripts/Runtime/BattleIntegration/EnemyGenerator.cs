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
    /// <param name="encounter"></param>
    public void SpawnEnemies(EncounterData encounter)
    {
        for (int i = 0; i < encounter.enemies.Count; i++)
        {
            EnemyData enemyData = encounter.enemies[i];
            Vector2 uiPos = encounter.uiPositions[i];

            EnemyUnit enemyUnit = new EnemyUnit();
            enemyUnit.Setup(enemyData);

            // UI生成
            EnemyUI enemyUI = Instantiate(enemyUnit.UIPrefab, _enemyArea).GetComponent<EnemyUI>();
            enemyUI.transform.localPosition = uiPos;
            enemyUI.Init(enemyUnit);

            // モデル生成（UIを基準に Y座標だけオフセット）
            EnemyModel enemyModel = Instantiate(enemyUnit.ModelPrefab, _enemyModelsArea).GetComponent<EnemyModel>();
            enemyModel.transform.localPosition = new Vector3(uiPos.x, _modelBaseY + enemyUnit.ModelYOffset, 0);
            enemyModel.Init(enemyUnit);

            // バインド
            enemyUnit.Model = enemyModel;
            enemyUnit.UI = enemyUI;

            EnemyManager.RegisterEnemy(enemyUnit);
        }
    }

}
