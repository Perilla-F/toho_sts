using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class BattleContext : IBattleContext
{
    public IBattleSystem BattleSystem { get; }
    public IHeroUnit Hero { get; private set; }
    public Hand Hand { get; private set; }
    public BattleDeck Deck { get; private set; }
    public DiscardArea Discard { get; private set; }
    public int Turn { get; private set; }

    public BattleContext(IHeroUnit hero, Hand hand, BattleDeck deck, DiscardArea discard)
    {
        Hero = hero;
        Hand = hand;
        Deck = deck;
        Discard = discard;
        Turn = 0;
    }

    public BattleUnit SelectTarget(BattleUnit enemy) => Hero;

    public void ProgressTurn()
    {
        Turn++;
    }
}
