using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface ICardEffect
{
    public CardEffectTarget TargetType { get; }
    public CardEffectType EffectType { get; }

    public void Apply(CardContext context);
    //UniTask ResolveAsync(CardContext context);
    //void Apply(IBattleUnit self, List<IBattleUnit> enemies, CardEffectTarget target);
}
