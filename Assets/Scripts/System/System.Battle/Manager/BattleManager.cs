using Cysharp.Threading.Tasks;

public class BattleManager
{
    private readonly GameManager _game;
    private readonly GameContext _gameContext;
    private readonly BattleSystem _battleSystem;
    private readonly IAudioManager _audio;
    private readonly BattleContext _context;
    private readonly EnemyManager _enemy;
    private readonly TimelineManager _timeline;
    private readonly PlayerController _controller;
    private readonly ICardUIHandler _hand;

    public BattleManager(GameManager game, GameContext gameContext, BattleContext battleContext, BattleSystem system, IAudioManager audio, EnemyManager enemy, PlayerController controller, TimelineManager timeline, ICardUIHandler hand)
    {
        _game = game;
        _gameContext = gameContext;
        _context = battleContext;
        _battleSystem = system;
        _audio = audio;
        _enemy = enemy;
        _timeline = timeline;
        _controller = controller;
        _hand = hand;
    }

    public void BattleSetUp()
    {
        var hero = new HeroUnit();
        hero.Setup(_gameContext.Hero);
        _battleSystem.Setup(_context, hero, _game, _enemy, _controller, _timeline, _hand, _audio);
    }

    public async UniTask StartBattle()
    {
        await _battleSystem.StartBattle();
        //_audio.PlayBGM("battle", true);
    }
}
