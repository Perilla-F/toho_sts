using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class BattleContext : IBattleContext
{
    public IHeroUnit Hero { get; private set; }
    public int Turn { get; private set; }
    public BattleSystem BattleSystem { get; private set; }
    public Hand Hand { get; private set; }
    public IHandView HandView { get; private set; }
    public IDeckView DeckView { get; private set; }
    public IBattleDeck BattleDeck { get; private set; }
    public ITimelineView TimelineView { get; private set; }
    public ITurnMessagePanel TurnMessagePanel { get; private set; }

    public BattleContext(BattleSystem battleSystem, IHeroUnit hero, IHandView handView, IDeckView deckView, ITurnMessagePanel turnMessagePanel)
    {
        BattleSystem = battleSystem;
        Hero = hero;
        HandView = handView;
        Turn = 0;
    }

    public Func<ICardObj, UniTask> OnCardDrawn;
    public Func<ICardObj, Transform, UniTask> MoveToHand;

    public IBattleSystem GetBattleSystem() => BattleSystem;
    public IHand GetHand() => Hand;
    public IHandView GetHandView() => HandView;
    public IBattleDeck GetBattleDeck() => BattleDeck;

    // 他の処理やユーティリティ

    public void ProgressTurn()
    {
        Turn++;
    }
}
