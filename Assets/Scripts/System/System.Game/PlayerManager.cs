using System.Collections.Generic;

public class PlayerManager : IPlayerManager
{
    private IGameManager _gameManager;

    public PlayerManager(IGameManager game)
    {
        _gameManager = game;
    }

    public void Inject(IGameManager game)
    {
        _gameManager = game;
    }

}