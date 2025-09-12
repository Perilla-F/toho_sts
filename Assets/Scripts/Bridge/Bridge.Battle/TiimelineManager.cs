using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager
{
    private List<BattleEvent> events = new List<BattleEvent>();

    public void AddEvent(BattleEvent e)
    {
        events.Add(e);
        SortEvents();
    }
    public BattleEvent PopNextEvent()
    {
        if (events.Count == 0) return null;
        events = events.OrderBy(e => e.Time).ThenBy(e => e.Priority).ToList();
        var next = events[0];
        events.RemoveAt(0);
        return next;
    }

    private void SortEvents()
    {
        events.Sort((a, b) =>
        {
            int cmp = a.Time.CompareTo(b.Time);
            if (cmp != 0) return cmp;

            // Type優先度: Player(0) < Boss(1) < Enemy(2)
            cmp = a.Type.CompareTo(b.Type);
            if (cmp != 0) return cmp;

            // 雑魚の左から順
            return a.Order.CompareTo(b.Order);
        });
    }

    /// <summary>
    /// 残りイベントを一斉消化
    /// </summary>
    public IEnumerator FlushAll(IBattleContext context)
    {
        while (events.Count > 0)
        {
            var e = PopNextEvent();
            e.Execute(context);

            // ここでアニメーション終了を待つ
            yield return new WaitUntil(() => e.IsFinished);

            // ちょっと間を置く演出
            yield return new WaitForSeconds(0.3f);

            events.Remove(e);
            e.Execute(context);
        }
    }

}
