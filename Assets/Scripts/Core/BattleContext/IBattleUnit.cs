using System;

public interface IBattlerUnit : IBattlerBaseData
{
    bool IsAlive();
    void TakeDamage(int amount);
    void Heal(int amount);
    void ApplyBlock(int amount);
    void ApplyStatus(String statusName, int amount);
    void ApplyAttackBuff(int amount);
    bool HasStatus(string status);
}