public class BattleManager
{
    private readonly BattleSystem _battleSystem;
    private readonly IAudioManager _audio;
    private readonly BattleContext _context;

    public BattleManager(BattleContext context, BattleSystem system, IAudioManager audio)
    {
        _context = context;
        _battleSystem = system;
        _audio = audio;
    }

    public void StartBattle()
    {
        _battleSystem.Initialize(_context);
        _audio.PlayBGM("battle", true);
    }
}
