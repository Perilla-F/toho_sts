using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;

public class TimelineView : MonoBehaviour
{
    [SerializeField] private Transform _iconContainer; // VerticalLayoutGroupがついた親
    [SerializeField] private GameObject _iconPrefab;
    private GameObject _previewInstance;
    private IReadOnlyTimelineManager _timelineManager;

    private List<TimelineIcon> _activeIcons = new List<TimelineIcon>();

    private void OnEnable()
    {
        // Managerからの通知を購読
        BattleEventBus.BattleEventAsync.OnActionsDecided += RefreshTimeline;
        BattleEventBus.BattleEventAsync.OnActionExecuted += RemoveTopEvent;
        BattleEventBus.BattleEventAsync.OnUpdateTime += UpdateIconCount;
        BattleEventBus.Card.OnCardExited += HidePreview;
    }

    public void Initialize(IReadOnlyTimelineManager timelineManager)
    {
        _timelineManager = timelineManager;
    }

    /// <summary>
    /// タイムライン初期化
    /// </summary>
    /// <param name="events"></param>
    public void RefreshTimeline(List<BattleEvent> events, bool playAnimation = true)
    {
        // 既存のアイコンをクリア（プール化するとより軽量）
        foreach (var icon in _activeIcons) Destroy(icon.gameObject);
        _activeIcons.Clear();

        for (int i = 0; i < events.Count; i++)
        {
            var obj = Instantiate(_iconPrefab, _iconContainer);
            var iconScript = obj.GetComponent<TimelineIcon>();

            // データをセット
            iconScript.Setup(events[i], _timelineManager.CurrentTime);

            if (playAnimation)
            {
                iconScript.PlaySpawnAnimation(i);
            }
            else
            {
                // アニメーションなしの場合は「表示状態」でセットするなどの処理
                iconScript.SetStaticState();
            }

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
            // 即座にイベントを停止
            _previewInstance.transform.DOKill();

            var cg = _previewInstance.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.DOKill();
            }

            // オブジェクトの破棄
            DestroyImmediate(_previewInstance);

            // 参照を即座にクリア
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
        icon.Setup(previewEvent, _timelineManager.CurrentTime);

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
        HidePreview();

        // 1. まずアニメーションなしでリスト全体を並べ替える
        RefreshTimeline(_timelineManager.GetUpcomingEvents(), playAnimation: false);

        // 2. 挿入されたアイコン（confirmedEvent）だけを探してアニメーションさせる
        var targetIcon = _activeIcons.FirstOrDefault(i => i.BattleEvent == confirmedEvent);
        if (targetIcon != null)
        {
            await targetIcon.PlayConfirmAnimation(ct);
        }
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

    private void UpdateIconCount()
    {
        foreach (var icon in _activeIcons)
        {
            icon.UpdateCountDown(icon.Count - _timelineManager.CurrentTime);
        }
    }

    // 3. アイコンの削除と詰め
    private void RemoveTopEvent(BattleEvent executedEvent)
    {
        if (_activeIcons.Count == 0) return;

        if (executedEvent is EnemyActionEvent ee)
        {
            Debug.Log($"EnemyUI: エネミーID{ee.EnemyId} の行動実行通知を受け取りました。");
        }
        var topIcon = _activeIcons[0];
        _activeIcons.RemoveAt(0);

        // ここでアニメーション（横にスライドして消えるなど）
        // アニメーション終了後にDestroy
        Destroy(topIcon.gameObject);
    }

    void OnDestroy()
    {
        BattleEventBus.BattleEventAsync.OnActionsDecided -= RefreshTimeline;
        BattleEventBus.BattleEventAsync.OnActionExecuted -= RemoveTopEvent;
        BattleEventBus.BattleEventAsync.OnUpdateTime -= UpdateIconCount;
        BattleEventBus.Card.OnCardExited -= HidePreview;
    }
}
