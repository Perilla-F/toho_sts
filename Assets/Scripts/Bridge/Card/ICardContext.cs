using System.Collections.Generic;

public interface ICardContext : IReadOnlyCardContext
{
    public IBattleSystem BattleSystem { get; }
    new IBattleUnit User { get; }
    new List<IBattleUnit> Targets { get; }
}

public interface IReadOnlyCardContext
{
    public IReadOnlyBattleUnit User { get; }
    public IReadOnlyList<IReadOnlyBattleUnit> Targets { get; }
}