using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;

public class TimelineView : MonoBehaviour
{
    [SerializeField] private Transform _iconContainer; // VerticalLayoutGroupがついた親
    [SerializeField] private GameObject _iconPrefab;
    private GameObject _previewInstance;
    private ITimelineManager _timelineManager;

    private List<TimelineIcon> _activeIcons = new List<TimelineIcon>();

    private void OnEnable()
    {
        // Managerからの通知を購読
        BattleEventBus.OnActionsDecided += RefreshTimeline;
        BattleEventBus.OnActionExecuted += RemoveTopEvent;
        BattleEventBus.OnCardExited += HidePreview;
    }

    public void Initialize(ITimelineManager timelineManager)
    {
        _timelineManager = timelineManager;
    }

    /// <summary>
    /// タイムライン初期化
    /// </summary>
    /// <param name="events"></param>
    public void RefreshTimeline(List<BattleEvent> events)
    {
        // 既存のアイコンをクリア（プール化するとより軽量）
        foreach (var icon in _activeIcons) Destroy(icon.gameObject);
        _activeIcons.Clear();

        for (int i = 0; i < events.Count; i++)
        {
            var obj = Instantiate(_iconPrefab, _iconContainer);
            var iconScript = obj.GetComponent<TimelineIcon>();

            // データをセット
            iconScript.Setup(events[i]);

            // アニメーション再生
            iconScript.PlaySpawnAnimation(i);

            _activeIcons.Add(iconScript);
        }
    }

    /// <summary>
    /// プレビュー取り消し
    /// </summary>
    public void HidePreview()
    {
        if (_previewInstance != null)
        {
            _previewInstance.transform.DOKill();

            var cg = _previewInstance.GetComponent<CanvasGroup>();
            if (cg != null) cg.DOKill();

            Destroy(_previewInstance);

            _previewInstance = null;
        }
    }

    /// <summary>
    /// プレビュー表示
    /// </summary>
    /// <param name="previewEvent"></param>
    public void ShowPreview(BattleEvent previewEvent)
    {
        HidePreview(); // 既にあったら消す

        var currentEvents = _timelineManager.GetUpcomingEvents();
        int insertIndex = CalculateInsertIndex(previewEvent, currentEvents);

        // プレビューアイコンを生成
        _previewInstance = Instantiate(_iconPrefab, _iconContainer);
        _previewInstance.transform.SetSiblingIndex(insertIndex);

        var icon = _previewInstance.GetComponent<TimelineIcon>();
        icon.Setup(previewEvent);

        // ここで点滅処理（CanvasGroupのDOTweenなど）を開始
        icon.StartBlinking();
    }

    /// <summary>
    /// 確定演出
    /// </summary>
    /// <param name="confirmedEvent"></param>
    /// <param name="insertIndex"></param>
    public async UniTask ConfirmAction(BattleEvent confirmedEvent, CancellationToken ct)
    {
        // 1. プレビューを即座に破棄
        HidePreview();

        // 2. 確定アイコンを生成
        var currentEvents = _timelineManager.GetUpcomingEvents();
        int insertIndex = CalculateInsertIndex(confirmedEvent, currentEvents);
        var obj = Instantiate(_iconPrefab, _iconContainer);
        obj.transform.SetSiblingIndex(insertIndex);
        var icon = obj.GetComponent<TimelineIcon>();
        icon.Setup(confirmedEvent);

        // 3. 挿入されたアイコンのみアニメーションさせる
        // 他のアイコンはLayoutGroupが自動的に押し出してくれるので、
        // 挿入されたアイコン自体をフェードやスケールで強調する
        await icon.PlayConfirmAnimation(ct);
    }

    private int CalculateInsertIndex(BattleEvent playerEvent, List<BattleEvent> currentEvents)
    {
        // TimelineManagerのSortEventsと同じロジックでシミュレーション
        int index = 0;
        foreach (var e in currentEvents)
        {
            if (CompareEvents(playerEvent, e) < 0) break;
            index++;
        }
        return index;
    }

    // TimelineManager.SortEvents と同じ比較ロジック
    private int CompareEvents(BattleEvent a, BattleEvent b)
    {
        int cmp = a.Time.CompareTo(b.Time);
        if (cmp != 0) return cmp;
        cmp = a.Type.CompareTo(b.Type);
        if (cmp != 0) return cmp;
        return a.Order.CompareTo(b.Order);
    }

    // 3. アイコンの削除と詰め
    private void RemoveTopEvent(BattleEvent executedEvent)
    {
        if (_activeIcons.Count == 0) return;

        Debug.Log($"EnemyUI: エネミーID{executedEvent.EnemyId} の行動実行通知を受け取りました。");
        var topIcon = _activeIcons[0];
        _activeIcons.RemoveAt(0);

        // ここでアニメーション（横にスライドして消えるなど）
        // アニメーション終了後にDestroy
        Destroy(topIcon.gameObject);
    }

    void OnDestroy()
    {
        BattleEventBus.OnActionsDecided -= RefreshTimeline;
        BattleEventBus.OnActionExecuted -= RemoveTopEvent;
        BattleEventBus.OnCardExited -= HidePreview;
    }
}
