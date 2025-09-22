using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStanbyState : IBattleState
{
    private BattleSystem _battle;
    public BattleStanbyState(BattleSystem battle)
    {
        this._battle = battle;
    }

    public async void OnEnter()
    {
        Debug.Log("StanbyのEnter");
        _battle.AddActionToTimeline();
        await _battle.BattleContext.GetTurnMessagePanel().ShowMessage($"{KanjiNumberConverteUtil.ConvertToKanjiWithUnits(_battle.TurnCount)}巡目");
        _battle.TransitionToState(BattleStateType.Draw);
    }

    public void OnExit()
    {
    }

    public void Update()
    {
    }
}
