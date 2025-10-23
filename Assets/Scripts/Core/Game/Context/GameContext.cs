public class GameContext
{
    public IGameManager GameManager;

    public GameContext(IGameManager gameManager)
    {
        GameManager = gameManager;
    }
}