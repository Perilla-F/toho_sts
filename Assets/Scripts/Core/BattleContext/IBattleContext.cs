public interface IBattleContext
{
    public IBattleSystem BattleSystem { get; }
    public IHeroUnit Hero { get; }
    public BattleUnit SelectTarget(BattleUnit enemy);
}