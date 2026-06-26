using System;
using System.Collections.Generic;

public static class StatusEffectFactory
{
    private static readonly Dictionary<string, Func<EffectData, int, IBattleUnit, StatusEffect>> _creators = new()
    {
        { "poison", (d, s, o) => new PoisonEffect(d, s, o) },
        { "fear", (d, s, o) => new FearEffect(d, s, o) },
        { "strength", (d, s, o) => new StrengthEffect(d, s, o)},
        { "defense", (d, s, o) => new DefenseEffect(d, s, o)}
    };

    public static StatusEffect Create(EffectData data, int stacks, IBattleUnit owner)
    {
        if (_creators.TryGetValue(data.EffectId, out var creator))
        {
            return creator(data, stacks, owner);
        }
        throw new Exception($"クラス {data.EffectId} が登録されていません");
    }
}
