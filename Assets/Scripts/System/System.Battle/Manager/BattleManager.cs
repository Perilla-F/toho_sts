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

    public BattleManager(GameManager game, GameContext gameContext, BattleContext battleContext, BattleSystem system, IAudioManager audio, EnemyManager enemy, PlayerController controller, TimelineManager timeline)
    {
        _game = game;
        _gameContext = gameContext;
        _context = battleContext;
        _battleSystem = system;
        _audio = audio;
        _enemy = enemy;
        _timeline = timeline;
        _controller = controller;
    }

    public void StartBattle()
    {
        var hero = new HeroUnit();
        hero.Setup(_gameContext.Player.HeroBattler);
        _battleSystem.Setup(_context, hero, _game, _enemy, _controller, _timeline, _audio);
        //_audio.PlayBGM("battle", true);
    }
}
