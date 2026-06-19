using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class HeroUnit : IHeroUnit
{
    public event Action<AnimationClip> OnAttack;
    public event Action<AnimationClip> OnHit;
    public event Action<int> OnManaChanged;

    public void Setup(HeroBattler heroBattler)
    {
        HPResource = heroBattler.HPResource;
        BattlerName = heroBattler.BaseData.BattlerName;
        AnimatorController = heroBattler.BaseData.AnimatorController;
        Mana = heroBattler.Mana;
        DrawCount = heroBattler.DrawCount;

        playerEventIcon = heroBattler.BaseData.playerEventIcon;

        UIPrefab = heroBattler.BaseData.UIPrefab;
        ModelPrefab = heroBattler.BaseData.ModelPrefab;
        ModelYOffset = heroBattler.BaseData.ModelYOffset;

        IdleClip = heroBattler.BaseData.IdleClip;
        AttackClip = heroBattler.BaseData.AttackClip;
        HitClip = heroBattler.BaseData.HitClip;

        Effects = new List<StatusEffect>();
    }

    public override async UniTask TakeDamageAsync(int amount)
    {
        await HPResource.TakeDamage(amount);
    }

    public override async UniTask Heal(int amount)
    {
        await HPResource.Gain(amount);
    }

    public override async UniTask ApplyBlock(int amount)
    {
        await HPResource.ApplyBlock(amount);
    }

    public override async UniTask ApplySimpleBlock(int amount)
    {
        await HPResource.ApplySimpleBlock(amount);
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

    public override async UniTask AddEffect(EffectData data, int stacks)
    {
        var existing = Effects.FirstOrDefault(e => e.Data.EffectId == data.EffectId);
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

    public override bool HasStatus(EffectData data)
    {
        return Effects.Find(e => e.Data.EffectId == data.EffectId) != null;
    }

    public override async UniTask GainMana(int amount)
    {
        await Mana.Gain(amount);
    }

    public override bool IsAlive()
    {
        return HPResource.GetHP() > 0;
    }
    public override bool IsDisabled()
    {
        return false;
    }
    public override int GetCurrentHP()
    {
        return HPResource.GetHP();
    }

    public override int GetMaxHP()
    {
        return HPResource.MaxHP;
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

    public override void Attack()
    {
        OnAttack?.Invoke(AttackClip);
    }

    public override void Hit()
    {
        OnHit?.Invoke(HitClip);
    }
}