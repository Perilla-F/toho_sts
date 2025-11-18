public static class BattleContextFactory
{
    public static BattleContext Create(EncounterData encounter, PlayerManager player, BattleViewRoot view, CardFactory factory)
    {
        var heroUnit = new HeroUnit();
        heroUnit.Setup(player.HeroBattler);

        var hand = new Hand();
        var deck = new BattleDeck();
        var discard = new DiscardArea();

        foreach (var source in player.PlayerDeck)
        {
            deck.AddCard(factory.CreateCard(source, view));
        }

        return new BattleContext(heroUnit, hand, deck, discard);
    }
}
