using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Schema;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattleSetUpState : IBattleState
{
    BattleSystem battle;
    public BattleSetUpState(BattleSystem battle)
    {
        this.battle = battle;
    }

    public void OnEnter()
    {
        Debug.Log("SetUpのEnter");
        battle.TransitionToState(BattleStateType.Stanby);
    }

    public void OnExit()
    {
    }

    public void Update()
    {
    }

}
