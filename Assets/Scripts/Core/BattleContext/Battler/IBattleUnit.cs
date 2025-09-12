using System;
using System.Collections.Generic;

public interface IBattleUnit : IBattlerBaseData
{
    public int Strength { get; set; }
    public int Defence { get; set; }
    public List<StatusEffect> Effects { get; }

    public abstract void TakeDamage(int amount);
    public abstract void Heal(int amount);
    public abstract void ApplyBlock(int amount);
    public abstract void ApplySimpleBlock(int amount);
    public abstract void AddEffect(StatusEffectData effect, int stacks);
    public abstract bool HasStatus(StatusEffectData data);
    public abstract bool IsAlive();
    public abstract bool IsDisabled();
    public abstract void ProcessTurnStart();
    public abstract void ProcessTurnEnd();
    public abstract int GetCurrentHP();
    public abstract int GetMaxHP();
}