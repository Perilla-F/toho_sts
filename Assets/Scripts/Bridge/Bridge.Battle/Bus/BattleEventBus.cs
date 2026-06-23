using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// System層とUI層の間でイベントを中継するブリッジクラス（イベントブローカー）。
/// 全てのイベントは静的に定義され、どこからでも購読・発行が可能。
/// </summary>
public static class BattleEventBus
{
    public static class Battle
    {
        /// <summary>
        /// バトル開始時に発行されるイベント(Mana, Deck, Discard)
        /// </summary>
        public static Action<IBattleContext, IMana> OnBattleStart;

        /// <summary>
        /// バトルが終了した時に発行されるイベント。
        /// </summary>
        public static Action<bool> OnBattleFinished; // true: 勝利, false: 敗北
    }

    public static class Turn
    {
        public static Action<int, CancellationToken> OnTurnStart;
        public static Action OnProcessEnd;
        public static Action OnTurnEnd;
    }

    public static class Card
    {
        public static Action<ICardObj, IBattleUnit, CancellationToken> OnCardUsed;
        public static Action<ICardObj, IBattleContext, CancellationToken> OnCardDrawn;

        /// <summary>
        /// UI上のカードがホバーされた時に発行されるイベント
        /// </summary>
        public static Action<ICardObj, IReadOnlyHeroUnit> OnCardHovered;

        /// <summary>
        /// UI上のカードがホバー解除された時に発行されるイベント
        /// </summary>
        public static Action OnCardExited;

        /// <summary>
        /// UI上のカードがアクティブ化した時に発行されるイベント
        /// </summary>
        public static Action<ICardObj> OnCardActive;

        public static Action<ICardObj, CancellationToken> OnDiscard;

        /// <summary>
        /// 全カードを待機状態にする時に発行されるイベント
        /// </summary>
        public static Action RestoreAllCards;
    }

    public static class BattleEventAsync
    {
        /// <summary>
        /// 敵・味方全ての次ターンの行動が決定し、タイムラインが構築された時に発行されるイベント。
        /// </summary>
        public static Action<List<BattleEvent>, bool> OnActionsDecided;

        /// <summary>
        /// タイムライン上の行動が実行された時に発行されるイベント。
        /// </summary>
        public static Action<BattleEvent> OnActionExecuted;

        /// <summary>
        /// タイムラインの時間を更新する際に発行されるイベント
        /// </summary>
        public static Action OnUpdateTime;

        /// <summary>
        /// カード演出のために発行されるイベント
        /// </summary>
        public static Action<CardData, IReadOnlyCardContext, UniTaskCompletionSource> OnCardUsed;

        /// <summary>
        /// 敵の攻撃演出のために発行されるイベント
        /// </summary>
        public static Action<IReadOnlyBattleUnit, UniTaskCompletionSource> OnEnemyAttackEffect;

    }

    public static class View
    {
        public static Action OnChangedHPCount;
        public static Action OnChangedManaCount;
        public static Action OnChangedDeckCount;
        public static Action OnChangedDiscardCount;
    }

}