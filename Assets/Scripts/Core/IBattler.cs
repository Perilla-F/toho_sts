public interface IBattler
{
    int MaxHP { get; }
    int CurrentHP { get; }
    int Attack { get; }
    int Defence { get; }
    float AttackModifier { get; }
    float DefenceModifier { get; }

    void TakeDamage(int amount);
    void ApplyBlock(int amount);
    void ApplyBuff(IBuff buff);
    int GetAttackBonus();
    int GetDefenceBonus();

}
