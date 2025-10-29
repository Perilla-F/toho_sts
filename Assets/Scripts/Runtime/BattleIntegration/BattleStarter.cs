// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BattleStarter : MonoBehaviour
// {
//     [SerializeField] private BattleSystem _battleSystem;
//     [SerializeField] private HandView _handView;
//     [SerializeField] private DeckView _deckView;
//     [SerializeField] private DiscardAreaView _discardAreaView;
//     [SerializeField] private TimelineView _timelineView;
//     [SerializeField] private ManaView _manaView;
//     [SerializeField] private TurnMessagePanel _turnMessagePanel;
//     [SerializeField] private EnemyGenerator _enemyGenerator;
//     [SerializeField] private Transform _heroArea;       // Canvas内 (UI)
//     [SerializeField] private Transform _heroModelArea;  // モデル用
//     [SerializeField] private float _heroBaseY = -200f;

//     private GameManager gameManager;
//     private SceneLoader sceneLoader;
//     private IAudioManager audioManager;

//     private void Start()
//     {
//         gameManager = ServiceLocator.Get<GameManager>();
//         sceneLoader = ServiceLocator.Get<SceneLoader>();

//         var encounterData = sceneLoader.GetTransitionData<BattleTransitionData>().EncounterData;
//         if (encounterData == null)
//         {
//             Debug.LogError("EncounterData が設定されていません！");
//             return;
//         }
//         StartBattle(encounterData);

//         audioManager = ServiceLocator.Get<IAudioManager>();
//     }

//     private void StartBattle(EncounterData encounterData)
//     {
//         if (gameManager == null || gameManager.Context.SelectedHeroData == null)
//         {
//             Debug.LogError("PlayerManager または selectedHeroData が null です。キャラ選択画面を経由してください。");
//             return;
//         }

//         HeroData playerHeroData = gameManager.Context.SelectedHeroData;
//         if (playerHeroData == null)
//         {
//             Debug.LogError("playerHeroData is null!");
//             return;
//         }

//         HeroUnit heroUnit = new HeroUnit();
//         heroUnit.Setup(gameManager.Context.HeroBattler);
//         HeroUI heroUI = Instantiate(playerHeroData.UIPrefab, _heroArea).GetComponent<HeroUI>();
//         heroUI.Init(heroUnit);
//         HeroModel model = Instantiate(playerHeroData.ModelPrefab, _heroModelArea).GetComponent<HeroModel>();
//         model.Init(heroUnit);

//         heroUnit.Model = model; // バインド

//         model.transform.position = new Vector3(-400, _heroBaseY, 0); // 固定配置

//         Hand hand = new Hand();
//         _handView.SetHand(hand);

//         List<SourceCard> playerDeck = gameManager.Context.PlayerDeck;
//         BattleDeck battleDeck = new BattleDeck();

//         var context = new BattleContext
//         (
//             _battleSystem,
//             heroUnit,
//             hand,
//             _handView,
//             _deckView,
//             _timelineView,
//             battleDeck,
//             _turnMessagePanel
//         );

//         _manaView.Init(heroUnit.Mana);

//         DiscardArea discardArea = new DiscardArea();
//         _discardAreaView.SetDiscardArea(discardArea);

//         foreach (var sourceCard in playerDeck)
//         {
//             CardObj cardObj = CardFactory.Instance.CreateCard(sourceCard, _deckView.transform, _deckView, _handView, _discardAreaView, _timelineView, sourceCard.SourceCost);
//             battleDeck.AddCard(cardObj);
//         }
//         battleDeck.Shuffle();
//         _deckView.SetBattleDeck(battleDeck);

//         _enemyGenerator.SpawnEnemies(encounterData);

//         TimelineManager timelineManager = new TimelineManager();
//         TimelineView timelineView = new TimelineView();
//         timelineView.Initialize(timelineManager, heroUnit);

//         // BattleSystemに渡す（DI）
//         _battleSystem.Setup(context, heroUnit, battleDeck, hand, discardArea, gameManager, timelineManager, audioManager);
//     }
// }
