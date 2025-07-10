using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStanbyState : IBattleState
{
    private BattleSystem battle;
    public BattleStanbyState(BattleSystem battle)
    {
        this.battle = battle;
    }

    public async void OnEnter()
    {
        Debug.Log("StanbyのEnter");
        battle.TimelineManager.ResetTime();
        battle.AddActionToTimeline();
        await battle.battleContext.TurnMessagePanel.ShowMessage($"{KanjiNumberConverteUtil.ConvertToKanjiWithUnits(battle.TurnCount)}巡目");
        battle.TransitionToState(BattleStateType.Draw);
    }

    public void OnExit()
    {
    }

    public void Update()
    {
    }
}
