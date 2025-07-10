using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class BattleContext : IBattleContext
{
    public BattleSystem BattleSystem { get; private set; }
    public Hand Hand { get; private set; }
    public IHandView HandView { get; private set; }
    public IDeckView DeckView { get; private set; }
    public IBattleDeck BattleDeck { get; private set; }
    public ITimelineView TimelineView { get; private set; }
    public ITurnMessagePanel TurnMessagePanel { get; private set; }

    public BattleContext(BattleSystem battleSystem, IHandView handView, IDeckView deckView, ITurnMessagePanel turnMessagePanel)
    {
        BattleSystem = battleSystem;
        HandView = handView;
    }

    public Func<ICardObj, UniTask> OnCardDrawn;
    public Func<ICardObj, Transform, UniTask> MoveToHand;

    public void ApplyStatus(IBattlerUnit target, string statusName, int amount)
    {
        target.ApplyStatus(statusName, amount);
    }

    public IBattleSystem GetBattleSystem() => BattleSystem;
    public IHand GetHand() => Hand;
    public IHandView GetHandView() => HandView;
    public IBattleDeck GetBattleDeck() => BattleDeck;

    // 他の処理やユーティリティ
}
