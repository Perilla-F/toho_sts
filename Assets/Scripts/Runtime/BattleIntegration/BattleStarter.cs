using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private HandView _handView;
    [SerializeField] private DeckView _deckView;
    [SerializeField] private DiscardAreaView _discardAreaView;
    [SerializeField] private TimelineView _timelineView;
    [SerializeField] private ManaView _manaView;
    [SerializeField] private TurnMessagePanel _turnMessagePanel;
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private Transform _heroArea;       // Canvas内 (UI)
    [SerializeField] private Transform _heroModelArea;  // モデル用
    [SerializeField] private float _heroBaseY = -200f;

    private void Start()
    {
        var encounterData = GameManager.Instance.CurrentEncounter;
        if (encounterData == null)
        {
            Debug.LogError("EncounterData が設定されていません！");
            return;
        }
        StartBattle(encounterData);
    }

    private void StartBattle(EncounterData encounterData)
    {
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
        HeroUI heroUI = Instantiate(playerHeroData.UIPrefab, _heroArea).GetComponent<HeroUI>();
        heroUI.Init(heroUnit);
        HeroModel model = Instantiate(playerHeroData.ModelPrefab, _heroModelArea).GetComponent<HeroModel>();
        model.Init(heroUnit);

        heroUnit.Model = model; // バインド

        model.transform.position = new Vector3(-400, _heroBaseY, 0); // 固定配置


        List<SourceCard> playerDeck = GameManager.Instance.GetPlayerDeck();
        BattleDeck battleDeck = new BattleDeck();

        var context = new BattleContext
        (
            _battleSystem,
            heroUnit,
            _handView,
            _deckView,
            _timelineView,
            battleDeck,
            _turnMessagePanel
        );

        Hand hand = new Hand();
        _handView.SetHand(hand);

        Mana mana = new Mana(heroUnit.MaxMana);
        _manaView.Init(mana);

        foreach (var sourceCard in playerDeck)
        {
            CardObj cardObj = CardFactory.Instance.CreateCard(sourceCard, _deckView.transform, _deckView, _handView, _discardAreaView, mana);
            battleDeck.AddCard(cardObj);
        }
        battleDeck.Shuffle();
        _deckView.SetBattleDeck(battleDeck);

        _enemyGenerator.SpawnEnemies(encounterData);

        TimelineManager timelineManager = new TimelineManager();
        TimelineView timelineView = new TimelineView();
        timelineView.Bind(timelineManager);

        // BattleSystemに渡す（DI）
        _battleSystem.Setup(context, heroUnit, battleDeck);
    }
}
