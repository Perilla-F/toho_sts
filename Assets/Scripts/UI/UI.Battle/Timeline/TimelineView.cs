using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimelineView : MonoBehaviour, ITimelineView
{
    [Header("References")]
    [SerializeField] private RectTransform timelineContainer; // タイムライン本体
    [SerializeField] private GameObject eventIconPrefab;      // 敵/プレイヤーのアイコンPrefab
    [SerializeField] private Image playerPredictionIcon;      // プレイヤーの行動予告アイコン
    [SerializeField] private Image playerPreviewIcon;

    [Header("Layout Settings")]
    [SerializeField] private float unitWidth = 50f; // 1時間単位の幅(px)
    [SerializeField] private float slideSpeed = 10f; // スライド速度(補間)

    private ITimelineManager _timeline;

    private int lastCurrentTime = 0;
    private float containerOffsetX = 0f;

    // --- 初期化 ---
    public void Initialize(IHeroUnit heroUnit, ITimelineManager timeline)
    {
        playerPredictionIcon = heroUnit.playerPredictionIcon;
        playerPreviewIcon = heroUnit.playerPreviewIcon;

        _timeline = timeline;
    }

    private void OnUpdate(int currentTime, int? predictedTime, int? previewTime)
    {
        UpdateTimelineSlide(currentTime);
        UpdateEventIcons();
        UpdatePlayerPrediction(predictedTime, previewTime);
    }

    /// <summary>
    /// タイムラインのスライド
    /// </summary>
    private void UpdateTimelineSlide(int currentTime)
    {
        if (currentTime != lastCurrentTime)
        {
            // 左端が現在時刻になるようにスライド
            float targetOffset = -currentTime * unitWidth;
            containerOffsetX = Mathf.Lerp(containerOffsetX, targetOffset, Time.deltaTime * slideSpeed);
            timelineContainer.anchoredPosition = new Vector2(containerOffsetX, 0);
            lastCurrentTime = currentTime;
        }
    }

    /// <summary>
    /// イベントアイコンの更新
    /// </summary>
    private void UpdateEventIcons()
    {
        foreach (Transform child in timelineContainer)
            Destroy(child.gameObject);

        var grouped = _timeline.GetUpcomingEvents()
            .GroupBy(e => e.Time)
            .OrderBy(g => g.Key);

        foreach (var group in grouped)
        {
            var first = group.First();
            var icon = Instantiate(eventIconPrefab, timelineContainer);
            var rect = icon.GetComponent<RectTransform>();

            // タイムライン上の位置
            float x = group.Key * unitWidth;
            rect.anchoredPosition = new Vector2(x, 0);

            // テキスト（×2など）
            var label = icon.GetComponentInChildren<TextMeshProUGUI>();
            if (group.Count() > 1)
                label.text = $"×{group.Count()}";
            else
                label.text = "";

            // アイコンの色区別（プレイヤー/敵など）
            var image = icon.GetComponent<Image>();
            switch (first.Type)
            {
                case EventType.Player:
                    image.color = Color.cyan;
                    break;
                case EventType.Boss:
                    image.color = Color.red;
                    break;
                case EventType.Enemy:
                    image.color = Color.yellow;
                    break;
            }
        }
    }

    /// <summary>
    /// プレイヤーの行動予告アイコン更新
    /// </summary>
    private void UpdatePlayerPrediction(int? predictedTime, int? previewTime)
    {
        // int? predictedTime = PlayerController.Instance?.PredictedActionTime;
        // int? previewTime = PlayerController.Instance?.PreviewActionTime;

        // --- 確定アイコン（不透明） ---
        if (predictedTime.HasValue)
        {
            playerPredictionIcon.enabled = true;
            playerPredictionIcon.color = new Color(0f, 1f, 1f, 1f); // 不透明シアン
            float x = predictedTime.Value * unitWidth;
            playerPredictionIcon.rectTransform.anchoredPosition = new Vector2(x, 50f);
        }
        else
        {
            playerPredictionIcon.enabled = false;
        }

        // --- プレビューアイコン（半透明） ---
        if (previewTime.HasValue)
        {
            playerPreviewIcon.enabled = true;
            playerPreviewIcon.color = new Color(0f, 1f, 1f, 0.4f); // 半透明
            float x = previewTime.Value * unitWidth;
            playerPreviewIcon.rectTransform.anchoredPosition = new Vector2(x, 50f);
        }
        else
        {
            playerPreviewIcon.enabled = false;
        }
    }
}
