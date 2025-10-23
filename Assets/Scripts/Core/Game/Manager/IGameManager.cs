using System.Collections.Generic;

public interface IGameManager
{
    public void InitializePlayer(HeroData heroData);
    public HeroBattler GetHeroBattler();
    public List<SourceCard> GetPlayerDeck();
    public void AddCard(SourceCard card);
    public void RemoveCard(SourceCard card);
    public void UpdateAfterBattle(List<SourceCard> updatedDeck);
}