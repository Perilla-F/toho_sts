using UnityEngine;
using DG.Tweening;

public class PopupScaler : MonoBehaviour
{
    public RectTransform PopupWindow;
    public float AnimationTime = 0.4f;

    private bool _isVisible = false;

    void Start()
    {
        // 初期状態：スケール0（見えない）
        PopupWindow.localScale = Vector3.zero;
    }

    public void ToggleWindow()
    {
        if (_isVisible)
        {
            PopupWindow.DOScale(Vector3.zero, AnimationTime).SetEase(Ease.InBack);
        }
        else
        {
            PopupWindow.DOScale(Vector3.one, AnimationTime).SetEase(Ease.OutBack);
        }
        _isVisible = !_isVisible;
    }
}
