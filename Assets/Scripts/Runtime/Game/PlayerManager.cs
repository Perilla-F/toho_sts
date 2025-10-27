using System.Collections.Generic;

public class PlayerManager : IPlayerManager
{
    private GameManager gameManager;
    private GameContext context;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        context = gameManager.Context;
    }

    public void InitializePlayer(HeroData selectedHeroData)
    {
        gameManager.InitializePlayer(selectedHeroData);
    }

    public HeroBattler GetHeroBattler()
    {
        return context.HeroBattler;
    }

    public List<SourceCard> GetPlayerDeck()
    {
        return context.PlayerDeck;
    }

    public PlayerSaveData CreateSaveData()
    {
        return new PlayerSaveData(
            context.HeroBattler,
            context.PlayerDeck
        );
    }

    public void LoadFromData(PlayerSaveData data)
    {
        context.HeroBattler = data.HeroBattler;
        context.PlayerDeck = data.PlayerDeck;
    }
}