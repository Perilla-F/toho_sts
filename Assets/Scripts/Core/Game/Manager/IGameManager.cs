using System.Collections.Generic;

public interface IGameManager
{
    public void InitializePlayer(HeroData heroData);
    public void AddCard(SourceCard card);
    public void RemoveCard(SourceCard card);
    public void UpdateDeckAfterBattle(List<SourceCard> updatedDeck);
    public void RequestSave();
}