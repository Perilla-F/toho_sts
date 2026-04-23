using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// System層とUI層の間でイベントを中継するブリッジクラス（イベントブローカー）。
/// 全てのイベントは静的に定義され、どこからでも購読・発行が可能。
/// </summary>
public static class BattleEventBus
{
    // ----------------------------------------------------
    // 1. 行動決定に関するイベント（System -> UI）
    // ----------------------------------------------------

    /// <summary>
    /// 敵・味方全ての次ターンの行動が決定し、タイムラインが構築された時に発行されるイベント。
    /// </summary>
    public static Action<List<BattleEvent>> OnActionsDecided;

    // ----------------------------------------------------
    // 2. 行動実行に関するイベント（System -> UI）
    // ----------------------------------------------------

    /// <summary>
    /// タイムライン上の行動が実行された時に発行されるイベント。
    /// </summary>
    public static Action<BattleEvent> OnActionExecuted;

    // ----------------------------------------------------
    // 3. バトル終了に関するイベント（System -> UI）
    // ----------------------------------------------------

    /// <summary>
    /// バトルが終了した時に発行されるイベント。
    /// </summary>
    public static Action<bool> OnBattleFinished; // true: 勝利, false: 敗北
}