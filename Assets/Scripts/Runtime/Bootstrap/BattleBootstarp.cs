using UnityEngine;

public class BattleBootstrap : MonoBehaviour
{

    [SerializeField] private BattleViewRoot _battleViewRoot; // View全体のまとめ
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private BattleSystem _battleSystem;

    private void Start()
    {
        var encounter = ServiceLocator.Get<SceneLoader>().GetTransitionData<BattleTransitionData>().EncounterData;
        var playerManager = ServiceLocator.Get<PlayerManager>();
        var audioManager = ServiceLocator.Get<IAudioManager>();

        var battleContext = BattleContextFactory.Create(encounter, playerManager);
        var battleManager = new BattleManager(battleContext, _battleSystem, audioManager);

        _enemyGenerator.SpawnEnemies(encounter);
        battleManager.StartBattle();
    }
}
