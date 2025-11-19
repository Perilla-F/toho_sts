using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameContext : IGameContext
{
    public HeroBattler Hero { get; private set; }
    public List<SourceCard> PlayerDeck { get; private set; }

    public Dictionary<Vector2Int, MapCellState> MapData { get; private set; }
    public Vector2Int CurrentCell { get; private set; }
    public int StageIndex { get; private set; }

    public string CurrentEventId { get; private set; }
    public string CurrentStepId { get; private set; }
    public bool CurrentEventCompleted { get; private set; }


    public GameContext()
    {
        PlayerDeck = new List<SourceCard>();
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
            new MapSaveData(MapData, CurrentCell.x, CurrentCell.y, StageIndex),
            new EventSaveData(CurrentEventId, CurrentStepId, CurrentEventCompleted)
        );
    }

    /// <summary>
    /// セーブデータから取得
    /// </summary>
    /// <param name="data"></param>
    public void FromSaveData(SaveData data)
    {
        Hero = data.Player.HeroBattler;
        PlayerDeck = data.Player.PlayerDeck;

        MapData = new Dictionary<Vector2Int, MapCellState>();
        foreach (var d in data.Map.mapData)
        {
            MapData.Add(d.Position, d.MapCellState);
        }
        CurrentCell = new Vector2Int(data.Map.cellX, data.Map.cellY);
        StageIndex = data.Map.stageIndex;

        CurrentEventId = data.Event.eventId;
        CurrentStepId = data.Event.stepId;
        CurrentEventCompleted = data.Event.isCompleted;
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

    public void SetCurrentCell(Vector2Int pos)
    {
        CurrentCell = pos;
    }

    public void SetStageIndex(int index)
    {
        StageIndex = index;
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