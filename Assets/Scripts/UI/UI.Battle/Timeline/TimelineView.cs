using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimelineView : MonoBehaviour
{
    [SerializeField] private Transform _iconContainer; // VerticalLayoutGroupがついた親
    [SerializeField] private GameObject _iconPrefab;

    private List<TimelineIcon> _activeBars = new List<TimelineIcon>();

    private void OnEnable()
    {
        // Managerからの通知を購読
        BattleEventBus.OnActionsDecided += RefreshTimeline;
        BattleEventBus.OnActionExecuted += RemoveTopEvent;
    }

    // 1. タイムラインの初期描画 / 全更新
    public void RefreshTimeline(List<BattleEvent> events)
    {
        // 既存のアイコンをクリア（プール化するとより軽量）
        foreach (var icon in _activeBars) Destroy(icon.gameObject);
        _activeBars.Clear();

        foreach (var e in events)
        {
            CreateIcon(e);
        }
    }

    private void CreateIcon(BattleEvent e)
    {
        var obj = Instantiate(_iconPrefab, _iconContainer);
        var actionBar = obj.GetComponent<TimelineIcon>();
        actionBar.Setup(e);
        _activeBars.Add(actionBar);
    }

    // 2. プレビュー表示（プレイヤーの行動選択中）
    public void ShowPlayerPreview(BattleEvent playerPotentialEvent, List<BattleEvent> currentEvents)
    {
        _iconPrefab.SetActive(true);

        // 予測されるTimeに基づいて、どこに挿入されるかインデックスを計算
        int insertIndex = CalculateInsertIndex(playerPotentialEvent, currentEvents);

        // VerticalLayoutGroup内での表示順を制御
        _iconPrefab.transform.SetSiblingIndex(insertIndex);
        // アイコンの中身（スキル名など）を更新
        _iconPrefab.GetComponent<TimelineIcon>().Setup(playerPotentialEvent);
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
        if (_activeBars.Count == 0) return;

        Debug.Log($"EnemyUI: エネミーID{executedEvent.EnemyId} の行動実行通知を受け取りました。");
        var topIcon = _activeBars[0];
        _activeBars.RemoveAt(0);

        // ここでアニメーション（横にスライドして消えるなど）
        // アニメーション終了後にDestroy
        Destroy(topIcon.gameObject);
    }
}
