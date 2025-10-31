using UnityEngine;

public class BattleBootstrap : MonoBehaviour
{
    [SerializeField] private CardFactoryConfig _cardFactoryConfig;
    [SerializeField] private BattleViewRoot _battleViewRoot; // View全体のまとめ
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private BattleSystem _battleSystem;

    private void Start()
    {
        var encounter = ServiceLocator.Get<SceneLoader>().GetTransitionData<BattleTransitionData>().EncounterData;
        var playerManager = ServiceLocator.Get<PlayerManager>();
        var audioManager = ServiceLocator.Get<IAudioManager>();

        var timeLineManager = new TimelineManager();
        var playerController = new PlayerController(timeLineManager);
        var enemyManager = new EnemyManager();
        var cardFactory = new CardFactory(_cardFactoryConfig, playerController);
        var battleContext = BattleContextFactory.Create(encounter, playerManager, _battleViewRoot, cardFactory);
        var battleManager = new BattleManager(battleContext, _battleSystem, audioManager, enemyManager, playerController);

        var battlePresenter = new BattlePresenter(_battleSystem, _battleViewRoot, playerController);

        _enemyGenerator.SpawnEnemies(encounter);
        battleManager.StartBattle();
    }
}
