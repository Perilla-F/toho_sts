using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Transform _enemyArea;        // Canvas内
    [SerializeField] private Transform _enemyModelsArea;  // モデル配置用
    [SerializeField] private float _modelBaseY = -200f;  // モデルのY初期位置
    [SerializeField] private EnemyUIEventChannel _enemyUIChannel;

    private int _nextEnemyId = 0;

    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="encounter"></param>
    public void SpawnEnemies(EncounterData encounter)
    {
        for (int i = 0; i < encounter.Enemies.Count; i++)
        {
            EnemyData enemyData = encounter.Enemies[i];
            Vector2 uiPos = encounter.UIPositions[i];

            // EnemyUnit生成・初期化
            EnemyUnit enemyUnit = new EnemyUnit();
            enemyUnit.Setup(enemyData);

            // ID付与
            int id = _nextEnemyId++;
            enemyUnit.EnemyID = id;

            // UI生成
            EnemyUI enemyUI = Instantiate(enemyUnit.UIPrefab, _enemyArea).GetComponent<EnemyUI>();
            enemyUI.transform.localPosition = uiPos;
            enemyUI.Bind(enemyUnit.HPResource);

            // モデル生成（UIを基準に Y座標だけオフセット）
            EnemyModel enemyModel = Instantiate(enemyUnit.ModelPrefab, _enemyModelsArea).GetComponent<EnemyModel>();
            enemyModel.transform.localPosition = new Vector3(uiPos.x, _modelBaseY + enemyUnit.ModelYOffset, 0);
            enemyModel.Init(enemyUnit, enemyUnit.IdleClip);

            // バインド
            enemyUnit.Model = enemyModel;
            enemyUnit.UI = enemyUI;

            // EventListenerを追加
            var listener = enemyUI.gameObject.AddComponent<EnemyUIEventListener>();
            listener.Initialize(enemyUnit.EnemyID, _enemyUIChannel, enemyUI);

            // EnemyControllerを生成・初期化
            var controller = enemyUI.gameObject.AddComponent<EnemyController>();
            controller.Initialize(enemyUnit.EnemyID, enemyData, _enemyUIChannel);

            // Bridgeに登録
            _enemyManager.RegisterEnemy(enemyUnit);
        }
    }

}
