using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class PlayerController
{
    public int? PredictedActionTime { get; private set; } = null;
    public int? PreviewActionTime { get; private set; } = null;

    private readonly TimelineManager _timeline;
    public bool HasChosenAction { get; private set; } = false;
    public bool TurnEndRequested { get; private set; } = false;
    public ICardObj ChosenCard { get; private set; }
    public CardContext CardContext { get; private set; }

    public Action<CancellationToken> ActionTurnEnd;
    private UniTaskCompletionSource _actionChoiceSource;
    private UniTaskCompletionSource _turnEndSource;

    public PlayerController(TimelineManager timeline)
    {
        _timeline = timeline;
    }

    /// <summary>
    /// 行動選択開始処理
    /// </summary>
    public void BeginSelection()
    {
        HasChosenAction = false;
        TurnEndRequested = false;
    }

    public async UniTask WaitForActionChosenAsync(CancellationToken ct)
    {
        _actionChoiceSource = new UniTaskCompletionSource();
        await _actionChoiceSource.Task;
    }

    // BattleManagerがカード使用を検知したとき
    public void SetChosenAction(ICardObj card, CardContext context)
    {
        ChosenCard = card;
        CardContext = context;
        _actionChoiceSource?.TrySetResult();
    }

    /// <summary>
    /// 行動確定時処理
    /// </summary>
    public void ConfirmAction()
    {
        HasChosenAction = false;
        ChosenCard = null;
    }

    /// <summary>
    /// ターン終了ボタン押下状態
    /// </summary>
    public void TurnEndButton()
    {
        TurnEndRequested = true;
    }

    public async UniTask WaitForTurnEndAsync(CancellationToken ct)
    {
        // 新しい待ち受け箱を作る
        _turnEndSource = new UniTaskCompletionSource();

        // 箱に結果が入るまで、ここで非同期に待機する
        await _turnEndSource.Task;
    }

    public void NotifyTurnEnd()
    {
        // 「ターン終了ボタンが押された」という結果を箱に入れる
        // これにより、WaitForTurnEndAsync の await が解除される
        _turnEndSource?.TrySetResult();
    }

    public void EndSelection()
    {
        // UIの非活性化処理（ボタンを隠すなど）
        // ここに直接書いても良いし、イベントを飛ばしても良い
    }

}
