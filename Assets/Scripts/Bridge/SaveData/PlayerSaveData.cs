using System.Collections.Generic;

[System.Serializable]
public class PlayerSaveData
{
    public HeroBattler HeroBattler;
    public List<SourceCard> PlayerDeck;
    public List<string> Flags;
    public EventSaveData LastEvent;

    public PlayerSaveData(HeroBattler heroBattler, List<SourceCard> playerDeck)
    {
        HeroBattler = heroBattler;
        PlayerDeck = playerDeck;
    }
}