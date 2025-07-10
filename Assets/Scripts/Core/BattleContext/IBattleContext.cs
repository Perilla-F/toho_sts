public interface IBattleContext
{
    IBattleSystem GetBattleSystem();
    IHand GetHand();
    IHandView GetHandView();
    IBattleDeck GetBattleDeck();

}
