using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestUIManager : MonoBehaviour
{
    [SerializeField] private GameObject restPanel;
    [SerializeField] private Button restButton;
    [SerializeField] private Button enhanceButton;

    public event Action OnRestPush;
    public event Action OnEnhancePush;

    public void Awake()
    {
        restPanel.SetActive(false);
    }

    public void RestPush()
    {
        OnRestPush?.Invoke();
    }

    public void EnhancePush()
    {
        OnEnhancePush?.Invoke();
    }

}
