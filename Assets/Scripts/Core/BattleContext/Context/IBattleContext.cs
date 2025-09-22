public interface IBattleContext
{
    public IHeroUnit Hero { get; }
    public int Turn { get; }
    IBattleSystem GetBattleSystem();
    IHand GetHand();
    IHandView GetHandView();
    IBattleDeck GetBattleDeck();
    BattleUnit SelectTarget(BattleUnit enemy);
    void ProgressTurn();

}
