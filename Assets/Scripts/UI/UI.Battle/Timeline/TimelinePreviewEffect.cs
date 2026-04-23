using UnityEngine;
using DG.Tweening;

public class TimelinePreviewEffect : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _canvasGroup.DOFade(0.3f, 1.0f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }

    void OnEnable()
    {
        _canvasGroup.alpha = 1.0f; // 初期化
    }

    void OnDisable()
    {
        _canvasGroup.alpha = 1.0f; // 初期化
    }
}