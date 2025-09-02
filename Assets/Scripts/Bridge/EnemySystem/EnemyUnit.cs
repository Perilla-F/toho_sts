using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyUnit : IBattleUnit, IEnemyUnit
{

    public string BattlerName { get; private set; }
    public float AttackModifier { get; } = 1f;
    public float DefenceModifier { get; } = 1f;
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }
    public int Strength { get; set; } = 0;
    public int Defence { get; set; } = 0;
    public int Block { get; private set; } = 0;
    public int AttackBonus { get; private set; } = 0;
    public int DefenceBonus { get; private set; } = 0;
    public List<string> Status = new List<string>();
    public List<IEffect> Effects { get; }

    public RuntimeAnimatorController AnimatorController { get; private set; }

    private EnemyAI _enemyAI;

    public void Setup(EnemyData data, EnemyAI enemyAI)
    {
        this.BattlerName = data.BattlerName;
        this._enemyAI = enemyAI;
        this.MaxHP = data.MaxHP;
        this.CurrentHP = data.MaxHP;
        this.AnimatorController = data.AnimatorController;
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

    public void ApplyStatus(String statusName, int amount)
    { }

    public void ApplyAttackBuff(int amount)
    {
        AttackBonus += amount;
    }

    public int GetAttackBonus()
    {
        return AttackBonus;
    }

    public int GetDefenceBonus()
    {
        return DefenceBonus;
    }

    public List<BattleAction> PlanTurn(int turn, ConditionContext context)
    {
        return _enemyAI.GetActions(this, turn, context);
    }

    public bool HasStatus(string status)
    {
        return this.Status.Contains(status);
    }

    public void ApplyEffect(IEffect effect)
    {
        Effects.Add(effect);
        effect.OnApply(this);
    }

    public bool IsAlive()
    {
        return CurrentHP > 0;
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
            e.OnTurnStart(this);
        }
    }

    public void ProcessTurnEnd()
    {
        foreach (var e in Effects)
        {
            e.OnTurnEnd(this);
        }
    }
}