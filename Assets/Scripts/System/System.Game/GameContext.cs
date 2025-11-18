using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameContext : IGameContext
{
    public PlayerManager Player { get; private set; }
    public MapManager Map { get; private set; }
    public EventManager Event { get; private set; }
    public HeroBattler Hero { get; private set; }
    public List<SourceCard> PlayerDeck { get; private set; }

    public Dictionary<Vector2Int, MapCellState> MapData { get; private set; }
    public Vector2Int CurrentCell { get; private set; }

    public string CurrentEventId { get; private set; }
    public string CurrentStepId { get; private set; }
    public bool CurrentEventCompleted { get; private set; }


    public GameContext(PlayerManager player, MapManager map, EventManager evt)
    {
        Player = player;
        Map = map;
        Event = evt;
        PlayerDeck = new List<SourceCard>();
    }

    public void InjectMap(MapManager map)
    {
        Map = map;
    }

    public void InjectEvent(EventManager evt)
    {
        Event = evt;
    }

    /// <summary>
    /// セーブデータへ変換
    /// </summary>
    /// <returns></returns>
    public SaveData ToSaveData()
    {
        return new SaveData
        (
            new PlayerSaveData(Hero, PlayerDeck),
            Map.CreateSaveData(),
            new EventSaveData(CurrentEventId, CurrentStepId, CurrentEventCompleted)
        );
    }

    /// <summary>
    /// セーブデータから復元
    /// </summary>
    /// <param name="data"></param>
    public void FromSaveData(SaveData data)
    {

    }

    public void SetHeroBattler(HeroBattler hero)
    {
        Hero = hero;
    }

    public void AddCard(SourceCard card)
    {
        PlayerDeck.Add(card);
    }

    public void RemoveCard(SourceCard card)
    {
        PlayerDeck.Remove(card);
    }

    public void UpdateDeck(List<SourceCard> updatedDeck)
    {
        PlayerDeck = new List<SourceCard>(updatedDeck);
    }

    public List<SourceCard> GetSourceCards()
    {
        return PlayerDeck;
    }

    public void SetMapData(Dictionary<Vector2Int, MapCellState> data)
    {
        MapData = data;
    }

    public void SetEventId(string id)
    {
        CurrentEventId = id;
    }

    public void SetEventStepId(string id)
    {
        CurrentStepId = id;
    }

    public void SetEventConpleted(bool isCompleted)
    {
        CurrentEventCompleted = isCompleted;
    }

}