using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private HandView _handView;
    [SerializeField] private DeckView _deckView;
    [SerializeField] private DiscardAreaView _discardAreaView;
    [SerializeField] private ManaView _manaView;
    [SerializeField] private HeroViewer _heroViewer;
    [SerializeField] private TurnMessagePanel _turnMessagePanel;
    [SerializeField] private EnemyGenerator _enemyGenerator;

    private void Start()
    {
        var context = new BattleContext
        (
            _battleSystem,
            _handView,
            _deckView,
            _turnMessagePanel
        );

        if (GameManager.Instance == null || GameManager.Instance.SelectedHeroData == null)
        {
            Debug.LogError("GameManager または selectedHeroData が null です。キャラ選択画面を経由してください。");
            return;
        }

        HeroData playerHeroData = GameManager.Instance.SelectedHeroData;
        if (playerHeroData == null)
        {
            Debug.LogError("playerHeroData is null!");
            return;
        }

        HeroUnit heroUnit = new HeroUnit();
        heroUnit.Setup(GameManager.Instance.HeroBattler);
        _heroViewer.ShowHero(playerHeroData, heroUnit);

        List<SourceCard> playerDeck = GameManager.Instance.GetPlayerDeck();
        BattleDeck battleDeck = new BattleDeck();

        Hand hand = new Hand();
        _handView.SetHand(hand);

        Mana mana = new Mana(heroUnit.MaxMana);
        _manaView.Init(mana);

        foreach (var sourceCard in playerDeck)
        {
            CardObj cardObj = CardFactory.CreateCard(sourceCard, _deckView.transform, _deckView, _handView, _discardAreaView, mana);
            battleDeck.AddCard(cardObj);
        }
        battleDeck.Shuffle();
        _deckView.SetBattleDeck(battleDeck);

        _enemyGenerator.SpawnEnemies("elite", heroUnit);

        TimelineManager timelineManager = new TimelineManager();
        TimelineView timelineView = new TimelineView();
        timelineView.Bind(timelineManager);

        // BattleSystemに渡す（DI）
        _battleSystem.Setup(context, heroUnit, battleDeck);
    }
}
