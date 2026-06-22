public interface IBattleContext : IReadOnlyBattleContext
{
    IBattleSystem BattleSystem { get; }
    new IHeroUnit Hero { get; }
    new IEnemyManager Enemies { get; }
    new IHand Hand { get; }
    new IBattleDeck Deck { get; }
    new IDiscardArea Discard { get; }
    new ITimelineManager Timeline { get; }
    public void ProgressTurn();
}

public interface IReadOnlyBattleContext
{
    IReadOnlyHeroUnit Hero { get; }
    IReadOnlyEnemyManager Enemies { get; }
    int Turn { get; }
    IReadOnlyBattleDeck Deck { get; }
    IReadOnlyHand Hand { get; }
    IReadOnlyDiscardArea Discard { get; }
    IReadOnlyTimelineManager Timeline { get; }
}