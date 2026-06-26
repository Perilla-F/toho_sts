using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BaseBattleUnit : IBattleUnit
{
    public string BattlerName { get; protected set; }
    public HPResource HPResource { get; protected set; }
    int IDamageable.CurrentHP => HPResource.GetHP();
    public List<StatusEffect> Effects { get; protected set; }
    public AnimationClip IdleClip { get; protected set; }
    public AnimationClip AttackClip { get; protected set; }
    public AnimationClip HitClip { get; protected set; }
    public AnimationClip BuffClip { get; protected set; }
    public DefenseComponent DefenseComponent { get; } = new DefenseComponent();

    public void Heal(int amount)
    {
        HPResource.Gain(amount);
    }

    public void ApplyBlock(int amount)
    {
        DefenseComponent.ApplyBlock(amount);
    }

    public void ApplySimpleBlock(int amount)
    {
        DefenseComponent.ApplySimpleBlock(amount);
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
            if (effect == null) return;
            BattleEventBus.View.OnUpdateBuffIcon(this, effect);
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
        DefenseComponent.ClearSimpleBlock();
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

    public virtual void Attack()
    {
    }

    public virtual void Buff()
    {

    }

    public virtual void Hit()
    {
    }

    public void UpdateHpBar()
    {
        BattleEventBus.View.OnUpdateHp?.Invoke(this);
    }

}