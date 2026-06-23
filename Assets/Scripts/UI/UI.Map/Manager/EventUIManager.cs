using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using TMPro;

public class EventUIManager : MonoBehaviour
{
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Transform optionRoot;
    [SerializeField] private Button optionButtonPrefab;
    [SerializeField] private Image eventImage;

    private UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite> currentImageHandle;
    public event Action<EventOption> OnOptionSelected;

    private readonly List<Button> currentButtons = new();

    public void Awake()
    {
        eventPanel.SetActive(false);
    }

    public void ShowStep(EventStep step)
    {
        eventPanel.SetActive(true);

        descriptionText.text = step.Text;

        // 古い画像の解放
        if (currentImageHandle.IsValid())
        {
            Addressables.Release(currentImageHandle);
            currentImageHandle = new UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite>(); // リセット
        }

        // 画像表示
        if (eventImage != null)
        {
            if (step.EventImageRef != null)
            {
                // 新しいHandleを保持
                currentImageHandle = step.EventImageRef.LoadAssetAsync<Sprite>();
                currentImageHandle.Completed += handle =>
                {
                    eventImage.sprite = handle.Result;
                };
            }
            else
                eventImage.sprite = null;
        }

        // 選択肢生成
        foreach (var b in currentButtons)
            Destroy(b.gameObject);
        currentButtons.Clear();

        foreach (var option in step.Options)
        {
            var btn = Instantiate(optionButtonPrefab, optionRoot);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = option.Text;
            btn.onClick.AddListener(() => OnOptionSelected?.Invoke(option));
            currentButtons.Add(btn);
        }
    }

    public void Hide() => eventPanel.SetActive(false);
}