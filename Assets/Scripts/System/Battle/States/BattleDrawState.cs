using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattleDrawState : IBattleState
{
    private BattleSystem _battle;
    public BattleDrawState(BattleSystem battle)
    {
        this._battle = battle;
    }

    public void OnEnter()
    {
        Debug.Log("DrawのEnter");
        int drawCardCount = _battle.HeroBattler.DrawCount;
        _battle.Draw(drawCardCount).Forget();
        _battle.TransitionToState(BattleStateType.CardSelection);
    }

    public void OnExit()
    {
    }

    public void Update()
    {
    }
}
