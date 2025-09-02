using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCardSelectionState : IBattleState
{
    private BattleSystem _battle;

    public BattleCardSelectionState(BattleSystem battle)
    {
        this._battle = battle;
    }

    public void OnEnter()
    {
        Debug.Log("SelectionのEnter");

        foreach (var card in _battle.Hand.Cards)
        {
            card.CardStateChange(CardStateName.CardWaitState);
        }
    }

    public void OnExit()
    {
        Debug.Log("SelectionのExit");
        foreach (var card in _battle.Hand.Cards)
        {
            card.CardStateChange(CardStateName.CardIdleState);
        }
    }

    public void Update()
    {
    }
}
