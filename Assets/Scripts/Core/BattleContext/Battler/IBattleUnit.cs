using System;
using System.Collections.Generic;

public interface IBattleUnit : IBattlerBaseData
{
    public int Strength { get; set; }
    public int Defence { get; set; }
    public List<IEffect> Effects { get; }
    public abstract bool IsAlive();
    public abstract void TakeDamage(int amount);
    public abstract void Heal(int amount);
    public abstract void ApplyBlock(int amount);
    public abstract void ApplyStatus(String statusName, int amount);
    public abstract void ApplyAttackBuff(int amount);
    public abstract bool HasStatus(string status);
    public abstract void ApplyEffect(IEffect effect);
    public abstract void ProcessTurnStart();
    public abstract void ProcessTurnEnd();
    public abstract int GetCurrentHP();
    public abstract int GetMaxHP();
}