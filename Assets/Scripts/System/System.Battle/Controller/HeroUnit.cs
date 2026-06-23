using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class HeroUnit : IHeroUnit
{
    public string BattlerName { get; private set; }
    public HPResource HPResource { get; private set; }
    int IDamageable.CurrentHP => HPResource.GetHP();
    public List<StatusEffect> Effects { get; private set; }
    public IBattleModel HeroModel { get; private set; }
    public AnimationClip IdleClip { get; private set; }
    public AnimationClip AttackClip { get; private set; }
    public AnimationClip HitClip { get; private set; }
    public AnimationClip BuffClip { get; private set; }
    public IBattleModel Model { get; private set; }
    public IBattleUI UI { get; private set; }
    public RuntimeAnimatorController AnimatorController { get; private set; }
    public DefenseComponent DefenseComponent { get; } = new DefenseComponent();

    public Sprite PlayerEventIcon { get; private set; }
    private Mana _mana;
    IMana IHeroUnit.Mana => _mana;
    IReadOnlyMana IReadOnlyHeroUnit.Mana => _mana;
    public int DrawCount { get; private set; }


    public void Setup(HeroBattler heroBattler)
    {
        HPResource = heroBattler.HPResource;
        BattlerName = heroBattler.BaseData.BattlerName;
        AnimatorController = heroBattler.BaseData.AnimatorController;
        _mana = heroBattler.Mana;
        DrawCount = heroBattler.DrawCount;

        PlayerEventIcon = heroBattler.BaseData.playerEventIcon;

        IdleClip = heroBattler.BaseData.IdleClip;
        AttackClip = heroBattler.BaseData.AttackClip;
        HitClip = heroBattler.BaseData.HitClip;

        Effects = new List<StatusEffect>();
    }

    public void BindUI(IBattleModel model, IBattleUI ui)
    {
        Model = model;
        UI = ui;
    }

    public void TakeDamageAsync(int amount)
    {
        HPResource.TakeDamage(amount);
    }

    public void Heal(int amount)
    {
        HPResource.Gain(amount);
    }

    public void ApplyBlock(int amount)
    {
        HPResource.ApplyBlock(amount);
    }

    public void ApplySimpleBlock(int amount)
    {
        HPResource.ApplySimpleBlock(amount);
    }

    public void AddEffect(EffectData data, int stacks)
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

    public bool HasStatus(string effectId)
    {
        return Effects.Find(e => e.Data.EffectId == effectId) != null;
    }

    public int StatusCount(string effectId)
    {
        if (Effects.Find(e => e.Data.EffectId == effectId) == null) return 0;
        return Effects.Find(e => e.Data.EffectId == effectId).Stacks;
    }

    public void GainMana(int amount)
    {
        _mana.Gain(amount);
    }

    public bool IsAlive()
    {
        return HPResource.GetHP() > 0;
    }
    public bool IsDisabled()
    {
        return false;
    }
    public int GetCurrentHP()
    {
        return HPResource.GetHP();
    }

    public int GetMaxHP()
    {
        return HPResource.MaxHP;
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

    public void Attack()
    {
    }

    public void Buff()
    {

    }

    public void Hit()
    {
    }
}