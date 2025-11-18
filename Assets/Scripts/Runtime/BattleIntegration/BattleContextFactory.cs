public static class BattleContextFactory
{
    public static BattleContext Create(EncounterData encounter, GameManager game, BattleViewRoot view, CardFactory factory)
    {
        var heroUnit = new HeroUnit();
        heroUnit.Setup(game.GetHeroBattler());

        var hand = new Hand();
        var deck = new BattleDeck();
        var discard = new DiscardArea();

        var sources = game.GetSourceCards();

        foreach (var source in sources)
        {
            deck.AddCard(factory.CreateCard(source, view));
        }

        return new BattleContext(heroUnit, hand, deck, discard);
    }
}
