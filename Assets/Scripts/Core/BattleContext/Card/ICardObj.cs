using System;
using Cysharp.Threading.Tasks;

public interface ICardObj
{
    public SourceCard Source { get; }
    public CardEffectTarget TargetType { get; }
    public int Delay { get; }
    bool IsSingleTarget => TargetType == CardEffectTarget.Enemy;
    public SourceCard GetSource();
    public UniTask Use(CardContext context);
    public bool Useable();
}
