using System;

public static class StatusEffectFactory
{
    public static StatusEffect Create(StatusEffectData data, int stacks, BattleUnit owner)
    {
        Type type = Type.GetType(data.className);
        if (type == null)
        {
            throw new Exception($"クラス {data.className} が見つかりませんでした");
        }

        return (StatusEffect)Activator.CreateInstance(type, data, stacks, owner);
    }
}
