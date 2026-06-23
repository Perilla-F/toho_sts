using UnityEngine;
using Live2D.Cubism.Framework.Motion;
using Live2D.Cubism.Framework.MotionFade;


public class HeroGenerator : MonoBehaviour
{
    [SerializeField] private Transform heroPosition;
    [SerializeField] private HeroUI heroUI;

    public HeroUnit GenerateHero(HeroBattler data)
    {
        var hero = new HeroUnit();
        hero.Setup(data);
        // 1. 生成
        var modelScale = new Vector3(data.BaseData.ModelScale, data.BaseData.ModelScale, data.BaseData.ModelScale);
        var modelObj = Instantiate(data.BaseData.ModelPrefab, heroPosition);
        modelObj.transform.localScale = modelScale;
        var motionController = modelObj.GetComponent<CubismMotionController>();
        var motionList = modelObj.GetComponent<CubismFadeController>().CubismFadeMotionList;
        if (motionController == null)
        {
            motionController = modelObj.AddComponent<CubismMotionController>();
        }

        var heroModel = modelObj.GetComponent<HeroModel>();
        heroModel.Initialize(data.BaseData, motionController, motionList);

        hero.BindUI(heroModel, heroUI);

        return hero;
    }
}