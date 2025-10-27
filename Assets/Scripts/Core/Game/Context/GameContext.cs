using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameContext
{
    public HeroData SelectedHeroData;
    public HeroBattler HeroBattler;
    public List<SourceCard> PlayerDeck;
    public EncounterData CurrentEncounter;
    public int StageIndex;
    public Vector2Int CurrentMapPosition;
    public List<string> AcquiredRelics;
    public MapSaveData MapSaveData;
    public EventSaveData EventSaveData;

    public GameContext()
    {
        PlayerDeck = new List<SourceCard>();
        AcquiredRelics = new List<string>();
    }
}