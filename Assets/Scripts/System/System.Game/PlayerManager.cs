using System.Collections.Generic;

public class PlayerManager
{
    private GameManager _gameManager;

    public PlayerManager(GameManager game)
    {
        _gameManager = game;
    }

    public void Inject(GameManager game)
    {
        _gameManager = game;
    }

}