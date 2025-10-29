using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class BattleContext
{
    public IBattleSystem BattleSystem { get; private set; }
    public IHeroUnit Hero { get; private set; }
    public int Turn { get; private set; }

    public BattleContext(IBattleSystem system, IHeroUnit hero)
    {
        BattleSystem = system;
        Hero = hero;
        Turn = 0;
    }

    public BattleUnit SelectTarget(BattleUnit enemy) => BattleSystem.Hero;

    public void ProgressTurn()
    {
        Turn++;
    }
}
