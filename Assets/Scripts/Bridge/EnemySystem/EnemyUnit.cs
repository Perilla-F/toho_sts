using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyUnit : IBattlerUnit, IEnemyUnit
{

    public string BattlerName { get; private set; }
    public float AttackModifier { get; } = 1f;
    public float DefenceModifier { get; } = 1f;
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }
    public int Attack { get; } = 0;
    public int Defence { get; } = 0;
    public int Block { get; private set; } = 0;
    public int AttackBonus { get; private set; } = 0;
    public int DefenceBonus { get; private set; } = 0;
    public List<string> status = new List<string>();

    public RuntimeAnimatorController AnimatorController { get; private set; }

    private EnemyAI enemyAI;

    public void Setup(EnemyData data, EnemyAI enemyAI)
    {
        this.BattlerName = data.BattlerName;
        this.enemyAI = enemyAI;
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

    public void ApplyBuff(IBuff buff) { /* バフ処理 */ }

    public List<BattleAction> PlanTurn(int turn, ConditionContext context)
    {
        return enemyAI.GetActions(this, turn, context);
    }

    public bool HasStatus(string status)
    {
        return this.status.Contains(status);
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
}