using System;

public static class StatusEffectFactory
{
    public static StatusEffect Create(EffectData data, int stacks, BattleUnit owner)
    {
        Type type = Type.GetType(data.ClassName);
        if (type == null)
        {
            throw new Exception($"クラス {data.ClassName} が見つかりませんでした");
        }

        return (StatusEffect)Activator.CreateInstance(type, data, stacks, owner);
    }
}
