using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    private EnemyManager _enemyManager;
    private Transform _enemyArea;
    private float _modelBaseY = -200f;  // モデルのY初期位置

    private int _nextEnemyId = 0;

    public EnemyGenerator(EnemyManager enemyManager, Transform enemyArea)
    {
        _enemyManager = enemyManager;
        _enemyArea = enemyArea;
    }

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
            EnemyModel enemyModel = Instantiate(enemyUnit.ModelPrefab, _enemyArea).GetComponent<EnemyModel>();
            enemyModel.transform.localPosition = new Vector3(uiPos.x, uiPos.y + _modelBaseY, -1f);
            enemyModel.transform.localScale = Vector3.one * 100f;
            enemyModel.Init(enemyUnit, enemyUnit.IdleClip);

            // バインド
            enemyUnit.Model = enemyModel;
            enemyUnit.UI = enemyUI;

            // EventListenerを追加
            EnemyUIEventListener listener = new EnemyUIEventListener();
            listener.Initialize(enemyUnit.EnemyID, enemyUI);

            // Bridgeに登録
            _enemyManager.RegisterEnemy(enemyUnit);
        }
    }

}
