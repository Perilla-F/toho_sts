using UnityEngine;
using DG.Tweening;

public class ExpandPanelDOTween : MonoBehaviour
{
    public RectTransform panel;
    public float duration = 0.25f;

    private bool isOpen = false;

    void Start()
    {
        panel.localScale = Vector3.zero;
    }

    public void TogglePanel()
    {
        isOpen = !isOpen;
        if (isOpen)
            panel.DOScale(Vector3.one, duration).SetEase(Ease.InQuad);
        else
            panel.DOScale(Vector3.zero, duration).SetEase(Ease.InQuad);
    }

    public void OpenPanel()
    {
        if (isOpen) return;
        isOpen = true;
        panel.DOScale(Vector3.one, duration).SetEase(Ease.InQuad);
    }

    public void ClosePanel()
    {
        if (!isOpen) return;
        isOpen = false;
        panel.DOScale(Vector3.zero, duration).SetEase(Ease.InQuad);
    }
}
