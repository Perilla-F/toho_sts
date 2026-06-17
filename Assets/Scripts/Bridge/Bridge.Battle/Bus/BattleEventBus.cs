using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// System層とUI層の間でイベントを中継するブリッジクラス（イベントブローカー）。
/// 全てのイベントは静的に定義され、どこからでも購読・発行が可能。
/// </summary>
public static class BattleEventBus
{
    /// <summary>
    /// 敵・味方全ての次ターンの行動が決定し、タイムラインが構築された時に発行されるイベント。
    /// </summary>
    public static Action<List<BattleEvent>> OnActionsDecided;

    /// <summary>
    /// UI上のカードがホバーされた時に発行されるイベント
    /// </summary>
    public static Action<ICardObj> OnCardHovered;

    /// <summary>
    /// UI上のカードがホバー解除された時に発行されるイベント
    /// </summary>
    public static Action OnCardExited;

    /// <summary>
    /// UI上のカードがアクティブ化した時に発行されるイベント
    /// </summary>
    public static Action<ICardObj> OnCardActive;

    /// <summary>
    /// 全カードを待機状態にする時に発行されるイベント
    /// </summary>
    public static Action RestoreAllCards;

    /// <summary>
    /// タイムライン上の行動が実行された時に発行されるイベント。
    /// </summary>
    public static Action<BattleEvent> OnActionExecuted;

    /// <summary>
    /// バトルが終了した時に発行されるイベント。
    /// </summary>
    public static Action<bool> OnBattleFinished; // true: 勝利, false: 敗北
}