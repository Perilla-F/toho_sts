using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class TurnMessagePanel : MonoBehaviour, ITurnMessagePanel
{
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI messageText;
    public async UniTask ShowMessage(String message)
    {
        gameObject.SetActive(true);
        //90度回転状態
        panel.transform.rotation = Quaternion.Euler(90, 0, 0);
        panel.SetActive(true);
        messageText.text = message;

        //0度回転状態に戻す
        await panel.transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        //0.3秒待機
        await UniTask.Delay(300);
        //90度回転状態に戻す
        panel.transform.DORotate(new Vector3(90, 0, 0), 0.5f).SetEase(Ease.Linear);
        gameObject.SetActive(false);
    }
}
