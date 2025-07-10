using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private HandView handView;
    [SerializeField] private DeckView deckView;
    [SerializeField] private DiscardAreaView discardAreaView;
    [SerializeField] private ManaView manaView;
    [SerializeField] private HeroViewer heroViewer;
    [SerializeField] private TurnMessagePanel turnMessagePanel;
    [SerializeField] private EnemyGenerator enemyGenerator;

    private void Start()
    {
        var context = new BattleContext
        (
            battleSystem,
            handView,
            deckView,
            turnMessagePanel
        );

        if (GameManager.Instance == null || GameManager.Instance.selectedHeroData == null)
        {
            Debug.LogError("GameManager または selectedHeroData が null です。キャラ選択画面を経由してください。");
            return;
        }

        HeroData playerHeroData = GameManager.Instance.selectedHeroData;
        if (playerHeroData == null)
        {
            Debug.LogError("playerHeroData is null!");
            return;
        }

        HeroUnit heroUnit = new HeroUnit();
        heroUnit.Setup(GameManager.Instance.heroBattler);
        heroViewer.ShowHero(playerHeroData, heroUnit);

        List<SourceCard> playerDeck = GameManager.Instance.GetPlayerDeck();
        BattleDeck battleDeck = new BattleDeck();

        Hand hand = new Hand();
        handView.SetHand(hand);

        Mana mana = new Mana(heroUnit.MaxMana);
        manaView.Init(mana);

        foreach (var sourceCard in playerDeck)
        {
            CardObj cardObj = CardFactory.CreateCard(sourceCard, deckView.transform, deckView, handView, discardAreaView, mana);
            battleDeck.AddCard(cardObj);
        }
        battleDeck.Shuffle();
        deckView.SetBattleDeck(battleDeck);

        enemyGenerator.SpawnEnemies("elite", heroUnit);

        TimelineManager timelineManager = new TimelineManager();
        TimelineView timelineView = new TimelineView();
        timelineView.Bind(timelineManager);

        // BattleSystemに渡す（DI）
        battleSystem.Setup(context, heroUnit, battleDeck);
    }
}
