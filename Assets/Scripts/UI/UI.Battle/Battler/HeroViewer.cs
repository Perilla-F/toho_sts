using UnityEngine;
using UnityEngine.UI;
using Live2D.Cubism.Framework.Motion; // CubismMotionController
using Live2D.Cubism.Framework;

public class HeroViewer : MonoBehaviour
{
    [SerializeField] private HeroArea heroArea; // モデルを配置するUIのTransform

    public HeroUnit HeroUnit { get; private set; }
    private GameObject currentModelInstance;
    private CubismMotionController motionController;

    public void ShowHero(IBattleHeroData heroData, HeroUnit heroUnit)
    {
        ClearCurrentModel();

        HeroUnit = heroUnit;

        if (heroData.Live2DModelPrefab == null)
        {
            Debug.LogError("Live2Dモデルが設定されていません。");
            return;
        }

        currentModelInstance = Instantiate(heroData.Live2DModelPrefab, heroArea.transform);
        currentModelInstance.transform.localPosition = Vector3.zero;
        currentModelInstance.transform.localScale = Vector3.one;

        motionController = currentModelInstance.GetComponent<CubismMotionController>();
        if (motionController != null && heroData.IdleMotionClip != null)
        {
            motionController.PlayAnimation(heroData.IdleMotionClip, isLoop: true);
        }
    }


    public void PlayMotion(AnimationClip motionClip, int layerIndex)
    {
        if (motionController != null)
        {
            motionController.PlayAnimation(motionClip, layerIndex);
        }
    }

    public void ClearCurrentModel()
    {
        if (currentModelInstance != null)
        {
            Destroy(currentModelInstance);
            currentModelInstance = null;
        }
    }

    public void UpdateHP(int current, int max)
    {
        heroArea.hpSlider.value = (float)current / max;
    }

}
