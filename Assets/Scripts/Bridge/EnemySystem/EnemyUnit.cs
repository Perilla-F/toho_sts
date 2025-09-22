using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyUnit : IBattleUnit
{
    public string BattlerName { get; private set; }
    public EnemyType EnemyType { get; private set; }
    public float AttackModifier { get; } = 1f;
    public float DefenceModifier { get; } = 1f;
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }
    public int Strength { get; set; } = 0;
    public int Defence { get; set; } = 0;
    public int Block { get; private set; } = 0;
    public int SimpleBlock { get; private set; } = 0;
    public int AttackBonus { get; private set; } = 0;
    public int DefenceBonus { get; private set; } = 0;
    public List<string> Status = new List<string>();
    public List<StatusEffect> Effects { get; }
    public ConditionType currentCondition = ConditionType.Turn;
    private ConditionType _lastCondition;
    private int _turnCounter = 0;

    public RuntimeAnimatorController AnimatorController { get; private set; }

    private EnemyAIData _enemyAI;

    public void Setup(EnemyData data)
    {
        BattlerName = data.BattlerName;
        _enemyAI = data.EnemyAI;
        MaxHP = data.MaxHP;
        CurrentHP = data.MaxHP;
        _lastCondition = currentCondition;
        EnemyType = data.EnemyType;
    }


    public void TakeDamage(int amount)
    {
        CurrentHP = Mathf.Max(0, CurrentHP - amount);
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(MaxHP, CurrentHP + amount);
    }

    public void ApplyBlock(int amount)
    {
        Block += amount;
    }

    public void ApplySimpleBlock(int amount)
    {
        SimpleBlock += amount;
    }

    public int GetAttackBonus()
    {
        return AttackBonus;
    }

    public int GetDefenceBonus()
    {
        return DefenceBonus;
    }

    /// <summary>
    /// AIから行動をターン中の行動をリストで引く
    /// </summary>
    /// <param name="turn"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public EnemyAction[] PlanTurn(IBattleContext context)
    {
        if (currentCondition != _lastCondition)
        {
            _turnCounter = 0;
            _lastCondition = currentCondition;
        }
        return _enemyAI.DecideActionPattern(context, this, _turnCounter);
    }

    public void AddEffect(StatusEffectData data, int stacks)
    {
        var existing = Effects.Find(e => e.Data.effectId == data.effectId);
        if (existing != null)
        {
            existing.AddStacks(stacks);
        }
        else
        {
            var effect = StatusEffectFactory.Create(data, stacks, this);
            Effects.Add(effect);
        }
    }

    public bool HasStatus(StatusEffectData data)
    {
        return Effects.Find(e => e.Data.effectId == data.effectId) != null;
    }

    public bool IsAlive()
    {
        return CurrentHP > 0;
    }
    public bool IsDisabled()
    {
        return false;
    }

    public int GetCurrentHP()
    {
        return CurrentHP;
    }

    public int GetMaxHP()
    {
        return MaxHP;
    }

    public void ProcessTurnStart()
    {
        foreach (var e in Effects)
        {
            e.OnTurnStart();
        }
    }

    public void ProcessTurnEnd()
    {
        foreach (var e in Effects)
        {
            e.OnTurnEnd();
        }
    }
}