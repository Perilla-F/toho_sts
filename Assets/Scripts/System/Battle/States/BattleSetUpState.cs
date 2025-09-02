using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Schema;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattleSetUpState : IBattleState
{
    private BattleSystem _battle;
    public BattleSetUpState(BattleSystem battle)
    {
        this._battle = battle;
    }

    public void OnEnter()
    {
        Debug.Log("SetUpのEnter");
        _battle.TransitionToState(BattleStateType.Stanby);
    }

    public void OnExit()
    {
    }

    public void Update()
    {
    }

}
