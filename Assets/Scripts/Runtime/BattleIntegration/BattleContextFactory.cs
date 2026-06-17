public static class BattleContextFactory
{
    public static BattleContext Create(EncounterData encounter, GameManager game, PlayerController player)
    {
        var heroUnit = new HeroUnit();
        heroUnit.Setup(game.GetHeroBattler());

        var hand = new Hand();
        var deck = new BattleDeck();
        var discard = new DiscardArea();

        var sources = game.GetSourceCards();

        foreach (var source in sources)
        {
            // 論理データ生成
            CardObj cardObj = new NomalCardObj(source, source.SourceCost, player);

            deck.AddCard(cardObj);
        }

        return new BattleContext(heroUnit, hand, deck, discard);
    }

}
