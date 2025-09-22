using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : IHeroUnit
{
    public string BattlerName { get; private set; }
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }
    public int MaxMana { get; private set; }
    public int CurrentMana { get; private set; }
    public Mana Mana;

    public float AttackModifier { get; } = 1f;
    public float DefenceModifier { get; } = 1f;
    public int Strength { get; set; } = 0;
    public int Defence { get; set; } = 0;
    public int Block { get; private set; } = 0;
    public int SimpleBlock { get; private set; }
    public int AttackBonus { get; private set; } = 0;
    public int DefenceBonus { get; private set; } = 0;
    public int DrawCount { get; private set; } = 5;
    public List<string> Status = new List<string>();
    public List<StatusEffect> Effects { get; }

    public RuntimeAnimatorController AnimatorController { get; private set; }

    public void Setup(HeroBattler heroBattler)
    {
        MaxHP = heroBattler.MaxHP;
        CurrentHP = heroBattler.CurrentHP;
        MaxMana = heroBattler.MaxMana;
        BattlerName = heroBattler.BaseData.BattlerName;
        AnimatorController = heroBattler.BaseData.AnimatorController;
        Mana = new Mana(MaxMana);
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

    public void ApplyAttackBuff(int amount)
    {
        AttackBonus += amount;
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

    public void GainMana(int amount)
    {
        Mana.Gain(amount);
    }

    public void Draw(int amount, IBattleContext context)
    {
        context.GetBattleSystem().Draw(amount);
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