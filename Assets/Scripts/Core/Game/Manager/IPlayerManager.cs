using System.Collections.Generic;

public interface IPlayerManager
{
    public void InitializePlayer(HeroData selectedHeroData);
    public HeroBattler GetHeroBattler();
    public List<SourceCard> GetPlayerDeck();
}