using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TimelineManager : ITimelineManager
{
    private List<BattleEvent> _events = new List<BattleEvent>();
    public int CurrentTime { get; set; }

    public void AddEvent(BattleEvent e)
    {
        Debug.Log($"追加するイベント: Time={e.Time}");
        foreach (var item in _events) { Debug.Log($"既存イベント: Time={item.Time}"); }
        _events.Add(e);
        SortEvents();
    }

    public async UniTask ExecuteNextEventAsync(IBattleContext context)
    {
        if (_events.Count == 0) return;

        var next = _events[0];
        _events.RemoveAt(0);

        CurrentTime = next.Time;

        // イベントの実行（計算＋演出）が終わるまで待つ
        await next.Execute(context);

        // 実行完了をバスで通知（UI更新用など）
        BattleEventBus.BattleEventAsync.OnActionExecuted?.Invoke(next);

        await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
    }

    /// <summary>
    /// 現在時刻から目標時刻までの間に、実行すべきイベントがあるか？
    /// </summary>
    /// <param name="targetTime"></param>
    /// <returns></returns>
    public bool HasEventsUntil(int targetTime)
    {
        var next = PeekNextEvent();
        return next != null && next.Time <= targetTime;
    }

    public bool HasEvents()
    {
        return _events.Count > 0;
    }


    /// <summary>
    /// 次のイベントを引っ張る
    /// </summary>
    /// <returns></returns>
    public BattleEvent PeekNextEvent()
    {
        if (_events.Count == 0) return null;
        return _events[0];
    }

    public List<BattleEvent> GetUpcomingEvents()
    {
        return new List<BattleEvent>(_events);
    }

    private void SortEvents()
    {
        // Sort()メソッドを使わずに、Linqで並び替えた「新しいリスト」を作る
        _events = _events.OrderBy(e => e.Time)
                         .ThenBy(e => e.Priority)
                         .ThenBy(e => e.Order)
                         .ToList();

        Debug.Log($"--- ソート完了後の並び ---");
        foreach (var e in _events) Debug.Log($"Time: {e.Time}");
    }

    /// <summary>
    /// ターン時間初期化
    /// </summary>
    public void RestTime()
    {
        CurrentTime = 0;
    }

    /// <summary>
    /// タイムラインの構築が完了した時に呼び出されるメソッド
    /// </summary>
    public void OnTimelineBuilt(bool isStart)
    {
        // Bridge層のイベントを呼び出し、UI層に通知する
        BattleEventBus.BattleEventAsync.OnActionsDecided?.Invoke(_events, isStart);
    }

}
