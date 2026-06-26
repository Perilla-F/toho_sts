using System.Collections.Generic;
using UnityEngine;

public interface IBattleUnit : IReadOnlyBattleUnit, IDamageable
{
    public void Heal(int amount);

    public void ApplyBlock(int amount);

    public void ApplySimpleBlock(int amount);

    public void AddEffect(EffectData data, int stacks);

    new bool HasStatus(string effectId);

    new int StatusCount(string effectId);

    public void ProcessTurnStart();

    public void ProcessTurnEnd();
    public void Attack();

    public void Hit();

}

public interface IReadOnlyBattleUnit
{
    public string BattlerName { get; }
    public HPResource HPResource { get; }
    public List<StatusEffect> Effects { get; }
    public AnimationClip IdleClip { get; }
    public AnimationClip AttackClip { get; }
    public AnimationClip HitClip { get; }
    public AnimationClip BuffClip { get; }
    public DefenseComponent DefenseComponent { get; }


    public bool HasStatus(string effectId);

    public int StatusCount(string effectId);

    public bool IsAlive();

    /// <summary>
    /// 行動不可
    /// </summary>
    /// <returns></returns>
    public bool IsDisabled();

    public int GetCurrentHP();

    public int GetMaxHP();

}
