public static class BattleContextFactory
{
    public static BattleContext Create(BattleSystem battleSystem, HeroUnit hero, GameManager game, IEnemyManager enemyManager, TimelineManager timelineManager)
    {
        var heroUnit = hero;

        var hand = new Hand();
        var deck = new BattleDeck();
        var discard = new DiscardArea();
        var selector = new EnemyTargetSelector();

        var sources = game.GetSourceCards();

        foreach (var source in sources)
        {
            // 論理データ生成
            CardObj cardObj = new NomalCardObj(source, source.SourceCost, selector);

            deck.AddCard(cardObj);
        }

        return new BattleContext(battleSystem, enemyManager, heroUnit, hand, deck, discard, timelineManager);
    }

}
