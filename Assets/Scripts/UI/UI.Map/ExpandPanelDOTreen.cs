using UnityEngine;
using DG.Tweening;

public class ExpandPanelDOTween : MonoBehaviour
{
    public RectTransform panel;
    public float duration = 0.4f;

    private bool isOpen = false;

    void Start()
    {
        panel.localScale = Vector3.zero;
    }

    public void TogglePanel()
    {
        isOpen = !isOpen;
        if (isOpen)
            panel.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
        else
            panel.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);
    }
}
