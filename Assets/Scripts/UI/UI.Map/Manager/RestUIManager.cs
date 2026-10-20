using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RestUIManager : MonoBehaviour
{
    [SerializeField] private GameObject restPanel;
    [SerializeField] private Transform buttonArea;
    [SerializeField] private Button restButton;
    [SerializeField] private Button enhanceButton;
    [SerializeField] private Button nextButton;


    public event Action OnRestPush;
    public event Action OnEnhancePush;
    public event Action OnEndRestPush;

    public void Awake()
    {
        restPanel.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        restPanel.SetActive(true);
        buttonArea.gameObject.SetActive(true);
    }

    public void Hide()
    {
        restPanel.SetActive(false);
    }

    public void EndEvent()
    {
        var nextRect = nextButton.gameObject.GetComponent<RectTransform>();
        var nextCanvas = nextButton.GetComponent<CanvasGroup>();
        buttonArea.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);

        nextRect.anchoredPosition = new Vector2(200f, 0);
        nextCanvas.alpha = 0f;

        nextRect.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutBack);
        nextCanvas.DOFade(1, 0.5f).SetEase(Ease.OutBack);
    }

    public void RestPush()
    {
        OnRestPush?.Invoke();
    }

    public void EnhancePush()
    {
        OnEnhancePush?.Invoke();
    }

    public void EndPush()
    {
        nextButton.gameObject.SetActive(false);
        OnEndRestPush?.Invoke();
    }

}
