using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BattleBootstrap : MonoBehaviour
{
    [SerializeField] private BattleViewRoot _battleViewRoot; // View全体のまとめ
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private BattleManager _battleManager;
    [SerializeField] private BattleCommandExecutor _commander;
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private GameObject cardPrefab;

    private CancellationToken _ct;

    private void Start()
    {
        _ct = this.GetCancellationTokenOnDestroy();

        var encounter = ServiceLocator.Get<ISceneLoader>().GetTransitionData<BattleTransitionData>().EncounterData;
        UnityEngine.Debug.Log("The encounter is " + encounter.EncounterID);
        var game = ServiceLocator.Get<GameManager>();
        var gameContext = ServiceLocator.Get<GameContext>();
        var audioManager = ServiceLocator.Get<IAudioManager>();

        var _timelineManager = new TimelineManager();
        var enemyManager = new EnemyManager();
        var CardPoolManager = new CardPoolManager(cardPrefab);
        _battleViewRoot.TimelineView.Initialize(_timelineManager);

        HeroUnit hero = new HeroUnit();
        hero.Setup(gameContext.Hero);
        var battleContext = BattleContextFactory.Create(_battleSystem, hero, game, enemyManager, _timelineManager);
        _battleSystem.Setup(battleContext, hero, enemyManager, _timelineManager, audioManager);

        var HandUIManager = new HandUIManager(_battleViewRoot, battleContext);
        HandUIManager.Setup(CardPoolManager);

        _battleManager.Initialize(_battleSystem, hero, enemyManager, _timelineManager, battleContext);
        _commander.Initialize(hero, battleContext, _battleSystem, enemyManager, _timelineManager, HandUIManager);

        _enemyGenerator.Init(enemyManager);
        _enemyGenerator.SpawnEnemies(encounter);
        _battleManager.BattleStart(_ct);
    }
}
