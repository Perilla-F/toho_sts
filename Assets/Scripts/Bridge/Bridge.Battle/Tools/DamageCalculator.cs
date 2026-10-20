using UnityEngine;

public static class DamageCalculator
{
    public static int CalculateDamage(IReadOnlyDamageable attacker, IReadOnlyDamageable defender, int baseDamage)
    {
        // 1. 攻撃側のバフ計算
        int finalDamage = baseDamage + attacker.StatusCount("strength");

        if (attacker.HasStatus("fear")) Mathf.FloorToInt(finalDamage * 0.75f);

        // 2. 防御側のデバフ計算
        if (defender.HasStatus("break")) Mathf.FloorToInt(finalDamage * 1.5f);

        return finalDamage;
    }
}