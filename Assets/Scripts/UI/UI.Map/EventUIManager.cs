using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using TMPro;

public class EventUIManager : MonoBehaviour, IEventView
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Transform optionRoot;
    [SerializeField] private Button optionButtonPrefab;
    [SerializeField] private Image eventImage;

    public event Action<EventOption> OnOptionSelected;

    private readonly List<Button> currentButtons = new();

    public void ShowStep(EventStep step)
    {
        gameObject.SetActive(true);

        descriptionText.text = step.Text;

        // 画像表示
        if (eventImage != null)
        {
            if (step.EventImageRef != null)
                step.EventImageRef.LoadAssetAsync<Sprite>().Completed += handle =>
                {
                    eventImage.sprite = handle.Result;
                };
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
            btn.GetComponentInChildren<Text>().text = option.Text;
            btn.onClick.AddListener(() => OnOptionSelected?.Invoke(option));
            currentButtons.Add(btn);
        }
    }

    public void Hide() => gameObject.SetActive(false);
}