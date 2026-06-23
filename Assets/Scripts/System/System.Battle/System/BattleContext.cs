public class BattleContext : IBattleContext
{
    private BattleSystem _battleSystem;
    IBattleSystem IBattleContext.BattleSystem => _battleSystem;
    private IEnemyManager _enemies;
    IEnemyManager IBattleContext.Enemies => _enemies;
    IReadOnlyEnemyManager IReadOnlyBattleContext.Enemies => _enemies;
    private HeroUnit _hero;
    IHeroUnit IBattleContext.Hero => _hero;
    IReadOnlyHeroUnit IReadOnlyBattleContext.Hero => _hero;
    private Hand _hand;
    IHand IBattleContext.Hand => _hand;
    IReadOnlyHand IReadOnlyBattleContext.Hand => _hand;
    private BattleDeck _deck;
    IBattleDeck IBattleContext.Deck => _deck;
    IReadOnlyBattleDeck IReadOnlyBattleContext.Deck => _deck;
    private DiscardArea _discard;
    IDiscardArea IBattleContext.Discard => _discard;
    IReadOnlyDiscardArea IReadOnlyBattleContext.Discard => _discard;
    private TimelineManager _timeline;
    ITimelineManager IBattleContext.Timeline => _timeline;
    IReadOnlyTimelineManager IReadOnlyBattleContext.Timeline => _timeline;
    public int Turn { get; private set; }

    public BattleContext(BattleSystem battleSystem, IEnemyManager enemies, HeroUnit hero, Hand hand, BattleDeck deck, DiscardArea discard, TimelineManager timeline)
    {
        _battleSystem = battleSystem;
        _enemies = enemies;
        _hero = hero;
        _hand = hand;
        _deck = deck;
        _discard = discard;
        _timeline = timeline;
        Turn = 0;
    }

    public void ProgressTurn()
    {
        Turn++;
    }
}
