using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public interface IBattleUnit : IReadOnlyBattleUnit
{

    public virtual void TakeDamageAsync(int amount)
    {
        HPResource.TakeDamage(amount);
    }

    public virtual void Heal(int amount)
    {
        HPResource.Gain(amount);
    }

    public virtual void ApplyBlock(int amount)
    {
        HPResource.ApplyBlock(amount);
    }

    public virtual void ApplySimpleBlock(int amount)
    {
        HPResource.ApplySimpleBlock(amount);
    }

    public virtual void AddEffect(EffectData data, int stacks)
    {
        var existing = Effects.Find(e => e.Data.EffectId == data.EffectId);
        if (existing != null)
        {
            existing.AddStacks(stacks);
            BuffUIChannel.OnBuffUpdated(existing);
        }
        else
        {
            var effect = StatusEffectFactory.Create(data, stacks, this);
            Effects.Add(effect);
            BuffUIChannel.OnBuffAdded(effect);
        }
    }

    public virtual void ProcessTurnStart()
    {
        foreach (var e in Effects)
        {
            e.OnTurnStart();
        }
    }

    public virtual void ProcessTurnEnd()
    {
        foreach (var e in Effects)
        {
            e.OnTurnEnd();
        }
    }

    public virtual void Attack() { }

    public virtual void Hit() { }

}

public interface IReadOnlyBattleUnit
{
    public string BattlerName { get; }
    public HPResource HPResource { get; }
    public List<StatusEffect> Effects { get; }
    public GameObject UIPrefab { get; }
    public GameObject ModelPrefab { get; }
    public float ModelYOffset { get; }
    public AnimationClip IdleClip { get; }
    public AnimationClip AttackClip { get; }
    public AnimationClip HitClip { get; }
    public AnimationClip BuffClip { get; }
    public IBattleModel Model { get; }
    public IBattleUI UI { get; }
    public RuntimeAnimatorController AnimatorController { get; }


    public virtual bool HasStatus(EffectData data)
    {
        return Effects.Find(e => e.Data.EffectId == data.EffectId) != null;
    }

    public virtual int StatusCount(string effectId)
    {
        if (Effects.Find(e => e.Data.EffectId == effectId) == null) return 0;
        return Effects.Find(e => e.Data.EffectId == effectId).Stacks;
    }

    public virtual bool IsAlive()
    {
        return HPResource.GetHP() > 0;
    }

    /// <summary>
    /// 行動不可
    /// </summary>
    /// <returns></returns>
    public virtual bool IsDisabled()
    {
        return false;
    }

    public virtual int GetCurrentHP()
    {
        return HPResource.GetHP();
    }

    public virtual int GetMaxHP()
    {
        return HPResource.MaxHP;
    }


}
