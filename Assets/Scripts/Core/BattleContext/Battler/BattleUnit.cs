using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnit
{
    public string BattlerName;
    public HPResource HPResource;
    public int Strength;
    public int Defence;
    public float AttackModifier { get; } = 1f;
    public float DefenceModifier { get; } = 1f;
    public int Block = 0;
    public int SimpleBlock = 0;
    public int AttackBonus = 0;
    public int DefenceBonus = 0;
    public List<string> Status = new List<string>();
    public List<StatusEffect> Effects { get; }

    public GameObject UIPrefab;    // HPバーなどのUIPrefab
    public GameObject ModelPrefab;  // Live2DモデルPrefab

    public float ModelYOffset;      // モデルのUI下の高さ調整

    //モーション
    public AnimationClip IdleClip;
    public AnimationClip AttackClip;
    public AnimationClip HitClip;

    public IBattleModel Model;
    public IBattleUI UI;


    public RuntimeAnimatorController AnimatorController { get; set; }

    public delegate void HpChanged(int newHp);
    public event HpChanged OnHpChanged;


    public abstract void TakeDamage(int amount);
    public abstract void Heal(int amount);
    public abstract void ApplyBlock(int amount);
    public abstract void ApplySimpleBlock(int amount);
    public abstract int GetAttackBonus();
    public abstract int GetDefenceBonus();
    public abstract void AddEffect(StatusEffectData effect, int stacks);
    public abstract bool HasStatus(StatusEffectData data);
    public abstract bool IsAlive();
    public abstract bool IsDisabled();
    public abstract void ProcessTurnStart();
    public abstract void ProcessTurnEnd();
    public abstract int GetCurrentHP();
    public abstract int GetMaxHP();
}