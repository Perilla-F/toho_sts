using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattleDrawState : IBattleState
{
    private BattleSystem battle;
    public BattleDrawState(BattleSystem battle)
    {
        this.battle = battle;
    }

    public void OnEnter()
    {
        Debug.Log("DrawのEnter");
        int drawCardCount = battle.heroBattler.DrawCount;
        battle.Draw(drawCardCount).Forget();
        battle.TransitionToState(BattleStateType.CardSelection);
    }

    public void OnExit()
    {
    }

    public void Update()
    {
    }
}
