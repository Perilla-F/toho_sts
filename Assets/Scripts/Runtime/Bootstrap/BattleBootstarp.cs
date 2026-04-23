using UnityEngine;
using Cysharp.Threading.Tasks;

public class BattleBootstrap : MonoBehaviour
{
    [SerializeField] private BattleViewRoot _battleViewRoot; // View全体のまとめ
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private Transform _enemyArea;
    [SerializeField] private GameObject cardPrefab;

    private async void Start()
    {
        var encounter = ServiceLocator.Get<ISceneLoader>().GetTransitionData<BattleTransitionData>().EncounterData;
        UnityEngine.Debug.Log("The encounter is " + encounter.EncounterID);
        var game = ServiceLocator.Get<GameManager>();
        var gameContext = ServiceLocator.Get<GameContext>();
        var playerManager = ServiceLocator.Get<PlayerManager>();
        var audioManager = ServiceLocator.Get<IAudioManager>();

        var timelineManager = new TimelineManager();
        var playerController = new PlayerController(timelineManager);
        var enemyManager = new EnemyManager();
        var CardPoolManager = new CardPoolManager(cardPrefab, playerController);
        var HandUIManager = new HandUIManager(_battleViewRoot);
        var CardFactory = new CardFactory(playerController);
        HandUIManager.Setup(CardPoolManager);
        var battleContext = BattleContextFactory.Create(encounter, game, playerController);
        var battleManager = new BattleManager(game, gameContext, battleContext, _battleSystem, audioManager, enemyManager, playerController, timelineManager, HandUIManager);
        battleManager.BattleSetUp();

        var battlePresenter = new BattlePresenter(_battleSystem, _battleViewRoot, playerController);

        EnemyGenerator _enemyGenerator = new EnemyGenerator(enemyManager, _enemyArea);
        _enemyGenerator.SpawnEnemies(encounter);
        await battleManager.StartBattle();
    }
}
