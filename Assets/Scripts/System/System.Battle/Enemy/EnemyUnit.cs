using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyUnit : IEnemyUnit
{
    private EnemyAIData _enemyAI;

    private EnemyUIEventChannel _uiChannel;

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
    }


    public override void TakeDamage(int amount)
    {
        HPResource.TakeDamage(amount);
    }

    public override void Heal(int amount)
    {
        HPResource.Gain(amount);
    }

    public override void ApplyBlock(int amount)
    {
        HPResource.ApplyBlock(amount);
    }

    public override void ApplySimpleBlock(int amount)
    {
        HPResource.ApplySimpleBlock(amount);
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
        _uiChannel?.Raise(new EnemyUIEventData
        {
            EnemyId = EnemyID,
            Type = EnemyUIEventType.ShowIntent,
            Icon = EventIcon
        });
        return _enemyAI.DecideActionPattern(context, this, turnCounter);
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