using UnityEngine;
using Cysharp.Threading.Tasks;

public class BattleBootstrap : MonoBehaviour
{
    [SerializeField] private BattleViewRoot _battleViewRoot; // View全体のまとめ
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private BattleManager _battleManager;
    [SerializeField] private TimelineManager _timelineManager;
    [SerializeField] private TimelineView _timelineView;
    [SerializeField] private Transform _enemyArea;
    [SerializeField] private GameObject cardPrefab;

    private void Start()
    {
        var encounter = ServiceLocator.Get<ISceneLoader>().GetTransitionData<BattleTransitionData>().EncounterData;
        UnityEngine.Debug.Log("The encounter is " + encounter.EncounterID);
        var game = ServiceLocator.Get<GameManager>();
        var gameContext = ServiceLocator.Get<GameContext>();
        var playerManager = ServiceLocator.Get<PlayerManager>();
        var audioManager = ServiceLocator.Get<IAudioManager>();

        var playerController = new PlayerController(_timelineManager);
        var enemyManager = new EnemyManager();
        var CardPoolManager = new CardPoolManager(cardPrefab, playerController);
        var HandUIManager = new HandUIManager(_battleViewRoot);
        HandUIManager.Setup(CardPoolManager);
        _battleViewRoot.TimelineView.Initialize(_timelineManager);
        var battleContext = BattleContextFactory.Create(encounter, game, playerController);

        HeroUnit hero = new HeroUnit();
        hero.Setup(gameContext.Hero);
        _battleSystem.Setup(battleContext, hero, game, enemyManager, playerController, _timelineManager, audioManager);

        _battleManager.Initialize(game, gameContext, hero, battleContext, _battleSystem, audioManager, enemyManager, playerController, _timelineManager, _timelineView, HandUIManager, _battleViewRoot);

        EnemyGenerator _enemyGenerator = new EnemyGenerator(enemyManager, _enemyArea);
        _enemyGenerator.SpawnEnemies(encounter);
        _battleManager.StartBattle();
    }
}
