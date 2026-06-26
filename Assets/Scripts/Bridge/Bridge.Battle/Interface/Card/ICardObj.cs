using System;
using Cysharp.Threading.Tasks;

public interface ICardObj : IReadOnlyCardObj
{
    public UniTask Use(ICardContext context);
}

public interface IReadOnlyCardObj
{
    public SourceCard Source { get; }
    public CardEffectTarget TargetType { get; }
    public int Delay { get; }
    public int SimpleBlock { get; }
    bool IsSingleTarget => TargetType == CardEffectTarget.Enemy;
    public SourceCard GetSource();
    public bool Useable();
}