using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class EnemyUnit : IEnemyUnit
{
    private EnemyAIData _enemyAI;

    public event Action<AnimationClip> OnAttack;
    public event Action<AnimationClip> OnHit;

    public void Setup(EnemyData data)
    {
        BattlerName = data.BattlerName;
        EventIcon = data.EventIcon;
        _enemyAI = data.EnemyAI;
        HPResource = new HPResource(data.MaxHP);
        lastCondition = currentCondition;
        EnemyType = data.EnemyType;
        UIPrefab = data.UIPrefab;
        ModelPrefab = data.ModelPrefab;
        ModelYOffset = data.ModelYOffset;
        IdleClip = data.IdleClip;
        AttackClip = data.AttackClip;
        HitClip = data.HitClip;

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

    /// <summary>
    /// AIから行動をターン中の行動をリストで引く
    /// </summary>
    /// <param name="turn"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public EnemyAction[] PlanTurn(BattleContext context)
    {
        if (currentCondition != lastCondition)
        {
            turnCounter = 0;
            lastCondition = currentCondition;
        }

        EnemyAction[] actions = _enemyAI.DecideActionPattern(context, this, turnCounter);
        return actions;
    }

    public override async UniTask AddEffect(EffectData data, int stacks)
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

    public override bool HasStatus(EffectData data)
    {
        return Effects.Find(e => e.Data.EffectId == data.EffectId) != null;
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