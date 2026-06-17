using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TimelineManager : MonoBehaviour, ITimelineManager
{
    private List<BattleEvent> _events = new List<BattleEvent>();
    public int CurrentTime { get; set; }

    public void AddEvent(BattleEvent e)
    {
        _events.Add(e);
        SortEvents();
    }

    public async UniTask ExecuteNextEventAsync(BattleContext context)
    {
        if (_events.Count == 0) return;

        var next = _events[0];
        _events.RemoveAt(0);

        CurrentTime = next.Time;

        // イベントの実行（計算＋演出）が終わるまで待つ
        await next.Execute(context);

        // 実行完了をバスで通知（UI更新用など）
        BattleEventBus.OnActionExecuted?.Invoke(next);

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
        return _events;
    }

    private void SortEvents()
    {
        _events.Sort((a, b) =>
        {
            int cmp = a.Time.CompareTo(b.Time);
            if (cmp != 0) return cmp;
            cmp = a.Priority.CompareTo(b.Priority);
            if (cmp != 0) return cmp;
            return a.Order.CompareTo(b.Order);
        });
    }

    /// <summary>
    /// タイムラインの構築が完了した時に呼び出されるメソッド
    /// </summary>
    public void OnTimelineBuilt()
    {
        // Bridge層のイベントを呼び出し、UI層に通知する
        BattleEventBus.OnActionsDecided?.Invoke(_events);
    }

}
