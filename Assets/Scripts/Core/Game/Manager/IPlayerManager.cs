using System.Collections.Generic;

public interface IPlayerManager
{
    public HeroBattler HeroBattler { get; }
    public void InitializePlayer(HeroData selectedHeroData);
    public void AddCard(SourceCard card);
    public void RemoveCard(SourceCard card);
    public void UpdateDeck(List<SourceCard> deck);
    public PlayerSaveData ToSaveData();
    public void LoadFrom(PlayerSaveData data);
}