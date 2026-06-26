using UnityEngine;
using Live2D.Cubism.Framework.Motion;
using Live2D.Cubism.Framework.MotionFade;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private Transform _uiPosition;
    [SerializeField] private Transform _modelPosition;
    [SerializeField] private Canvas _canvas;

    private EnemyManager _enemyManager;
    private ModelRegistory _modelRegistory;

    private int _nextEnemyId = 0;

    public void Init(EnemyManager enemyManager, ModelRegistory modelRegistory)
    {
        _enemyManager = enemyManager;
        _modelRegistory = modelRegistory;
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
            EnemyUI enemyUI = Instantiate(enemyData.UIPrefab, _uiPosition).GetComponent<EnemyUI>();
            enemyUI.transform.localPosition = uiPos;
            enemyUI.Bind(enemyUnit);

            var modelObj = Instantiate(enemyData.ModelPrefab, enemyUI.transform);
            modelObj.transform.localPosition += Vector3.up * enemyData.ModelYOffset;
            modelObj.transform.localScale = modelScale;
            var motionController = modelObj.GetComponent<CubismMotionController>();
            var motionFade = modelObj.GetComponent<CubismFadeController>().CubismFadeMotionList;
            if (motionController == null)
            {
                motionController = modelObj.AddComponent<CubismMotionController>();
            }
            var enemyModel = modelObj.GetComponent<EnemyModel>();
            enemyModel.Initialize(enemyUnit, enemyData, motionController, motionFade);

            // Bridgeに登録
            _enemyManager.RegisterEnemy(id, enemyUnit);
            _modelRegistory.RegisterModel(enemyUnit, enemyModel);
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
