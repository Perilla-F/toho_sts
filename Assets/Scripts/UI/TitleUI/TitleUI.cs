using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button continueButton;

    public Action OnContinue;

    public void Setup(bool hasSave)
    {
        continueButton.interactable = hasSave;
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    public void OnClickContinue()
    {
        OnContinue?.Invoke();
    }
}