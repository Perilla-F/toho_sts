using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class EnemyUnit : IEnemyUnit
{
    public string BattlerName { get; private set; }
    public HPResource HPResource { get; private set; }
    int IDamageable.CurrentHP => HPResource.GetHP();
    public List<StatusEffect> Effects { get; private set; }
    public AnimationClip IdleClip { get; private set; }
    public AnimationClip AttackClip { get; private set; }
    public AnimationClip HitClip { get; private set; }
    public AnimationClip BuffClip { get; private set; }
    public IBattleModel Model { get; private set; }
    public IBattleUI UI { get; private set; }
    public RuntimeAnimatorController AnimatorController { get; }
    public DefenseComponent DefenseComponent { get; } = new DefenseComponent();

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
    }

    public void SetID(int id)
    {
        EnemyID = id;
    }

    public void BindUI(IBattleModel model, IBattleUI ui)
    {
        Model = model;
        UI = ui;
    }

    public bool HasStatus(string effectId)
    {
        return Effects.Find(e => e.Data.EffectId == effectId) != null;
    }

    public int StatusCount(string effectId)
    {
        if (Effects.Find(e => e.Data.EffectId == effectId) == null) return 0;
        return Effects.Find(e => e.Data.EffectId == effectId).Stacks;
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