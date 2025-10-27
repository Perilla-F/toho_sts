using System.Collections.Generic;

public interface IGameManager
{
    public GameContext Context { get; }

    public void InitializePlayer(HeroData heroData);
    public void AddCard(SourceCard card);
    public void RemoveCard(SourceCard card);
    public void UpdateDeckAfterBattle(List<SourceCard> updatedDeck);
}