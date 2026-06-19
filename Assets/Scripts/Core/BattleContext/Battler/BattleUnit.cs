using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class BattleUnit
{
    public string BattlerName;
    public HPResource HPResource;
    public int Strength;
    public int Defence;
    public float AttackModifier { get; } = 1f;
    public float DefenceModifier { get; } = 1f;
    public int AttackBonus = 0;
    public int DefenceBonus = 0;
    public List<string> Status = new List<string>();
    public List<StatusEffect> Effects { get; set; }

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

    // public delegate void HpChanged(int newHp);
    // public event HpChanged OnHpChanged;

    public abstract UniTask TakeDamageAsync(int amount);
    public abstract UniTask Heal(int amount);
    public abstract UniTask ApplyBlock(int amount);
    public abstract UniTask ApplySimpleBlock(int amount);
    public abstract int GetAttackBonus();
    public abstract int GetDefenceBonus();
    public abstract UniTask AddEffect(EffectData effect, int stacks);
    public abstract bool HasStatus(EffectData data);
    public abstract bool IsAlive();
    public abstract bool IsDisabled();
    public abstract void ProcessTurnStart();
    public abstract void ProcessTurnEnd();
    public abstract int GetCurrentHP();
    public abstract int GetMaxHP();
    public abstract void Attack();
    public abstract void Hit();
}