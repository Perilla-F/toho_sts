using UnityEngine;

public class BattleBootstrap : MonoBehaviour
{
    [SerializeField] private CardFactoryConfig _cardFactoryConfig;
    [SerializeField] private BattleViewRoot _battleViewRoot; // View全体のまとめ
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private BattleSystem _battleSystem;

    private void Start()
    {
        var encounter = ServiceLocator.Get<ISceneLoader>().GetTransitionData<BattleTransitionData>().EncounterData;
        UnityEngine.Debug.Log(encounter.EncounterID);
        var game = ServiceLocator.Get<GameManager>();
        var gameContext = ServiceLocator.Get<GameContext>();
        var playerManager = ServiceLocator.Get<PlayerManager>();
        var audioManager = ServiceLocator.Get<IAudioManager>();

        var timelineManager = new TimelineManager();
        var playerController = new PlayerController(timelineManager);
        var enemyManager = new EnemyManager();
        var cardFactory = new CardFactory(_cardFactoryConfig, playerController);
        var battleContext = BattleContextFactory.Create(encounter, game, _battleViewRoot, cardFactory);
        var battleManager = new BattleManager(game, gameContext, battleContext, _battleSystem, audioManager, enemyManager, playerController, timelineManager);

        var battlePresenter = new BattlePresenter(_battleSystem, _battleViewRoot, playerController);

        _enemyGenerator.SpawnEnemies(encounter);
        battleManager.StartBattle();
    }
}
