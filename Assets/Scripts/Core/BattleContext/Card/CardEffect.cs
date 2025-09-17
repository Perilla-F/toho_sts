using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public abstract class CardEffect
{
    public CardEffectTarget TargetType { get; }
    public CardEffectType EffectType { get; }

    public abstract void Apply(CardContext context);
    //UniTask ResolveAsync(CardContext context);
    //void Apply(IBattleUnit self, List<IBattleUnit> enemies, CardEffectTarget target);
}
