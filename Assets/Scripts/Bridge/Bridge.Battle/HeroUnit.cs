using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : IBattlerUnit, IHeroUnit
{
    public string BattlerName { get; private set; }
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }
    public int MaxMana { get; private set; }

    public float AttackModifier { get; } = 1f;
    public float DefenceModifier { get; } = 1f;
    public int Attack { get; } = 0;
    public int Defence { get; } = 0;
    public int Block { get; private set; } = 0;
    public int AttackBonus { get; private set; } = 0;
    public int DefenceBonus { get; private set; } = 0;
    public int DrawCount { get; private set; } = 5;
    public List<string> status = new List<string>();

    public RuntimeAnimatorController AnimatorController { get; private set; }

    public void Setup(HeroBattler heroBattler)
    {
        this.MaxHP = heroBattler.MaxHP;
        this.CurrentHP = heroBattler.CurrentHP;
        this.MaxMana = heroBattler.MaxMana;
        this.BattlerName = heroBattler.BaseData.BattlerName;
        this.AnimatorController = heroBattler.BaseData.AnimatorController;
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

    public void ApplyStatus(String statusName, int amouint)
    { }

    public int GetAttackBonus()
    {
        return AttackBonus;
    }

    public int GetDefenceBonus()
    {
        return DefenceBonus;
    }

    public void ApplyAttackBuff(int amount)
    {
        AttackBonus += amount;
    }

    public bool HasStatus(string status)
    {
        return this.status.Contains(status);
    }

    public bool IsAlive()
    {
        return CurrentHP > 0;
    }
}