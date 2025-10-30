public class BattleManager
{
    private readonly BattleSystem _battleSystem;
    private readonly IAudioManager _audio;
    private readonly BattleContext _context;
    private readonly EnemyManager _enemy;
    private readonly PlayerController _controller;

    public BattleManager(BattleContext context, BattleSystem system, IAudioManager audio, EnemyManager enemy, PlayerController controller)
    {
        _context = context;
        _battleSystem = system;
        _audio = audio;
        _enemy = enemy;
        _controller = controller;
    }

    public void StartBattle()
    {
        _battleSystem.Init(_enemy, _controller);
        _audio.PlayBGM("battle", true);
    }
}
