using UnityEngine;
using DG.Tweening;

public class PopupScaler : MonoBehaviour
{
    public RectTransform popupWindow;
    public float animationTime = 0.4f;

    private bool isVisible = false;

    void Start()
    {
        // 初期状態：スケール0（見えない）
        popupWindow.localScale = Vector3.zero;
    }

    public void ToggleWindow()
    {
        if (isVisible)
        {
            popupWindow.DOScale(Vector3.zero, animationTime).SetEase(Ease.InBack);
        }
        else
        {
            popupWindow.DOScale(Vector3.one, animationTime).SetEase(Ease.OutBack);
        }
        isVisible = !isVisible;
    }
}
