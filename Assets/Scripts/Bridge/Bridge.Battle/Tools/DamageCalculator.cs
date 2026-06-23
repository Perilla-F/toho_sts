using UnityEngine;

public static class DamageCalculator
{
    public static int CalculateDamage(IDamageable attacker, IDamageable defender, int baseDamage)
    {
        // 1. 攻撃側のバフ計算
        float multiplier = 1.0f;
        int finalDamage = baseDamage + attacker.StatusCount("strength");

        if (attacker.HasStatus("fear")) multiplier *= 0.75f;

        // 2. 防御側のデバフ計算
        if (defender.HasStatus("break")) multiplier *= 1.5f;

        return Mathf.FloorToInt(finalDamage * multiplier);
    }
}