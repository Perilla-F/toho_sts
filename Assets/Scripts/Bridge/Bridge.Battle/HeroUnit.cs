using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : IHeroUnit
{

    public int MaxMana { get; private set; }
    public int CurrentMana { get; private set; }
    public Mana Mana;

    public int DrawCount { get; private set; } = 5;

    public void Setup(HeroBattler heroBattler)
    {
        MaxHP = heroBattler.MaxHP;
        CurrentHP = heroBattler.CurrentHP;
        MaxMana = heroBattler.MaxMana;
        BattlerName = heroBattler.BaseData.BattlerName;
        AnimatorController = heroBattler.BaseData.AnimatorController;
        Mana = new Mana(MaxMana);

        UIPrefab = heroBattler.BaseData.UIPrefab;
        ModelPrefab = heroBattler.BaseData.ModelPrefab;
        ModelYOffset = heroBattler.BaseData.ModelYOffset;

        IdleClip = heroBattler.BaseData.IdleClip;
        AttackClip = heroBattler.BaseData.AttackClip;
        HitClip = heroBattler.BaseData.HitClip;
    }

    public override void TakeDamage(int amount)
    {
        CurrentHP = Mathf.Max(0, CurrentHP - amount);
    }

    public override void Heal(int amount)
    {
        CurrentHP = Mathf.Min(MaxHP, CurrentHP + amount);
    }

    public override void ApplyBlock(int amount)
    {
        Block += amount;
    }

    public override void ApplySimpleBlock(int amount)
    {
        SimpleBlock += amount;
    }

    public override int GetAttackBonus()
    {
        return AttackBonus;
    }

    public override int GetDefenceBonus()
    {
        return DefenceBonus;
    }

    public void ApplyAttackBuff(int amount)
    {
        AttackBonus += amount;
    }

    public override void AddEffect(StatusEffectData data, int stacks)
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

    public override bool HasStatus(StatusEffectData data)
    {
        return Effects.Find(e => e.Data.effectId == data.effectId) != null;
    }

    public override void GainMana(int amount)
    {
        Mana.Gain(amount);
    }

    public override void Draw(int amount, IBattleContext context)
    {
        context.GetBattleSystem().Draw(amount);
    }

    public override bool IsAlive()
    {
        return CurrentHP > 0;
    }
    public override bool IsDisabled()
    {
        return false;
    }
    public override int GetCurrentHP()
    {
        return CurrentHP;
    }

    public override int GetMaxHP()
    {
        return MaxHP;
    }

    public override void ProcessTurnStart()
    {
        foreach (var e in Effects)
        {
            e.OnTurnStart();
        }
    }

    public override void ProcessTurnEnd()
    {
        foreach (var e in Effects)
        {
            e.OnTurnEnd();
        }
    }
}