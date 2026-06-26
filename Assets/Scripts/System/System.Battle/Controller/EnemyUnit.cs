using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class EnemyUnit : BaseBattleUnit, IEnemyUnit
{
    public EnemyType EnemyType { get; private set; }
    public ConditionType currentCondition { get; private set; }
    public ConditionType lastCondition { get; private set; }
    public int turnCounter { get; private set; }
    public Sprite EventIcon { get; private set; }
    public int EnemyID { get; private set; }
    private EnemyAIData _enemyAI;

    public void Setup(EnemyData data)
    {
        BattlerName = data.BattlerName;
        EventIcon = data.EventIcon;
        _enemyAI = data.EnemyAI;
        HPResource = new HPResource(data.MaxHP);
        lastCondition = currentCondition;
        EnemyType = data.EnemyType;
        IdleClip = data.IdleClip;
        AttackClip = data.AttackClip;
        HitClip = data.HitClip;
        BuffClip = data.BuffClip;

        Effects = new List<StatusEffect>();
        DefenseComponent.Block = 0;
        DefenseComponent.SimpleBlock = 0;
    }

    public void SetID(int id)
    {
        EnemyID = id;
    }

    /// <summary>
    /// AIから行動をターン中の行動をリストで引く
    /// </summary>
    /// <param name="turn"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public EnemyAction[] PlanTurn(IBattleContext context)
    {
        if (currentCondition != lastCondition)
        {
            turnCounter = 0;
            lastCondition = currentCondition;
        }

        EnemyAction[] actions = _enemyAI.DecideActionPattern(context, this, turnCounter);
        return actions;
    }

}