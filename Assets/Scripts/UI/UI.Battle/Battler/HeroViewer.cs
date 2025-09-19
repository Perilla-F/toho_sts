using UnityEngine;
using UnityEngine.UI;
using Live2D.Cubism.Framework.Motion; // CubismMotionController
using Live2D.Cubism.Framework;

public class HeroViewer : MonoBehaviour
{
    [SerializeField] private HeroArea _heroArea;

    public HeroUnit HeroUnit { get; private set; }
    private GameObject _currentModelInstance;
    private CubismMotionController _motionController;

    public void ShowHero(IBattleHeroData heroData, HeroUnit heroUnit)
    {
        ClearCurrentModel();

        HeroUnit = heroUnit;

        if (heroData.Live2DModelPrefab == null)
        {
            Debug.LogError("Live2Dモデルが設定されていません。");
            return;
        }

        _currentModelInstance = Instantiate(heroData.Live2DModelPrefab, _heroArea.transform);
        _currentModelInstance.transform.localPosition = Vector3.zero;
        _currentModelInstance.transform.localScale = Vector3.one;

        _motionController = _currentModelInstance.GetComponent<CubismMotionController>();
        if (_motionController != null && heroData.IdleMotionClip != null)
        {
            _motionController.PlayAnimation(heroData.IdleMotionClip, isLoop: true);
        }
    }

    public void PlayMotion(AnimationClip motionClip, int layerIndex)
    {
        if (_motionController != null)
        {
            _motionController.PlayAnimation(motionClip, layerIndex);
        }
    }

    public void ClearCurrentModel()
    {
        if (_currentModelInstance != null)
        {
            Destroy(_currentModelInstance);
            _currentModelInstance = null;
        }
    }

    public void UpdateHP(int current, int max)
    {
        _heroArea.HpSlider.value = (float)current / max;
    }

}
