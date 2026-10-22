using System.Collections.Generic;
using UnityEngine;

public interface IGameContext
{
    public HeroBattler Hero { get; }
    public int StageIndex { get; }
    public Dictionary<Vector2Int, MapCellState> MapData { get; }
    public Vector2Int CurrentCell { get; }
    public string CurrentEventId { get; }
    public string CurrentStepId { get; }
    public bool CurrentEventCompleted { get; }


    public void SetHeroBattler(HeroBattler hero);
    public void AddCard(SourceCard card);
    public void RemoveCard(SourceCard card);
    public void UpdateDeck(List<SourceCard> updatedDeck);
    public List<SourceCard> GetSourceCards();
    public void SetMapData(Dictionary<Vector2Int, MapCellState> data);
    public void SetStageIndex(int index);
    public void SetCurrentCell(Vector2Int pos);
    public void SetEventId(string id);

    public void SetEventStepId(string id);

    public void SetEventConpleted(bool isCompleted);
    public SaveData ToSaveData();
    public void FromSaveData(SaveData data);

}