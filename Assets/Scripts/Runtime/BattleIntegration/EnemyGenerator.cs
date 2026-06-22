using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private Transform _uiPosition;
    [SerializeField] private Transform _modelPosition;
    [SerializeField] private Canvas _canvas;

    private EnemyManager _enemyManager;
    private float _modelBaseY = 30f;  // モデルのY初期位置

    private int _nextEnemyId = 0;

    public void Init(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
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
            Vector3 modelScale = new Vector3(encounter.ModelScale[i], encounter.ModelScale[i], encounter.ModelScale[i]);

            // EnemyUnit生成・初期化
            EnemyUnit enemyUnit = new EnemyUnit();
            enemyUnit.Setup(enemyData);

            // ID付与
            int id = _nextEnemyId++;
            enemyUnit.SetID(id);

            // 1. UIの生成
            EnemyUI enemyUI = Instantiate(enemyUnit.UIPrefab, _uiPosition).GetComponent<EnemyUI>();
            enemyUI.transform.localPosition = uiPos;
            enemyUI.Bind(enemyUnit.HPResource);

            // 2. モデルの生成を「WorldRoot」のようなCanvas外のTransformにする
            Vector3 targetWorldPos = ConvertUiPosToWorldPos(uiPos);
            EnemyModel enemyModel = Instantiate(enemyUnit.ModelPrefab, _modelPosition).GetComponent<EnemyModel>();
            enemyModel.transform.position = new Vector3(targetWorldPos.x, targetWorldPos.y + _modelBaseY, targetWorldPos.z);
            enemyModel.transform.localScale = modelScale;
            enemyModel.Init(enemyUnit, enemyUnit.IdleClip);

            // バインド
            enemyUnit.BindUI(enemyModel, enemyUI);

            // EventListenerを追加
            EnemyUIEventListener listener = new EnemyUIEventListener();
            listener.Initialize(enemyUnit.EnemyID, enemyUI);

            // Bridgeに登録
            _enemyManager.RegisterEnemy(enemyUnit);
        }
    }

    private Vector3 ConvertUiPosToWorldPos(Vector2 uiPos)
    {
        // 1. UIのLocalPositionを、現在のカメラからのワールド座標に変換する
        // Camera.mainはシーンのメインカメラ
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(_canvas.worldCamera, _uiPosition.TransformPoint(uiPos));

        // 2. スクリーン座標をワールド座標へ（Z=5はカメラからの距離）
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, 5f));

        return worldPos;
    }

}
