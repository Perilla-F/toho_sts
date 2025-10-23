using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class BattleContext
{
    public IHeroUnit Hero { get; private set; }
    public int Turn { get; private set; }
    private IBattleSystem BattleSystem;
    private IHand Hand;
    private IHandView HandView;
    private IDeckView DeckView;
    private IBattleDeck BattleDeck;
    private ITimelineView TimelineView;
    private ITurnMessagePanel TurnMessagePanel;

    public BattleContext(IBattleSystem battleSystem, IHeroUnit hero, IHand hand, IHandView handView, IDeckView deckView, ITimelineView timelineView, IBattleDeck battleDeck, ITurnMessagePanel turnMessagePanel)
    {
        BattleSystem = battleSystem;
        Hero = hero;
        Hand = hand;
        HandView = handView;
        DeckView = deckView;
        BattleDeck = battleDeck;
        TimelineView = timelineView;
        TurnMessagePanel = turnMessagePanel;
        Turn = 0;
    }

    public Func<ICardObj, UniTask> OnCardDrawn;
    public Func<ICardObj, Transform, UniTask> MoveToHand;

    public IBattleSystem GetBattleSystem() => BattleSystem;
    public IHand GetHand() => Hand;
    public IHandView GetHandView() => HandView;
    public IDeckView GetDeckView() => DeckView;
    public IBattleDeck GetBattleDeck() => BattleDeck;
    public ITimelineView GetTimelineView() => TimelineView;
    public ITurnMessagePanel GetTurnMessagePanel() => TurnMessagePanel;
    public BattleUnit SelectTarget(BattleUnit enemy) => Hero;

    // 他の処理やユーティリティ

    public void ProgressTurn()
    {
        Turn++;
    }
}
