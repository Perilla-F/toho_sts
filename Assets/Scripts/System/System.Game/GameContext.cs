using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameContext : IGameContext
{
    public PlayerManager Player { get; private set; }
    public MapManager Map { get; private set; }
    public EventManager Event { get; private set; }
    public HeroBattler Hero { get; private set; }

    public GameContext(PlayerManager player, MapManager map, EventManager evt)
    {
        Player = player;
        Map = map;
        Event = evt;
        Hero = Player.HeroBattler;
    }

    /// <summary>
    /// セーブデータへ変換
    /// </summary>
    /// <returns></returns>
    public SaveData ToSaveData()
    {
        return new SaveData
        (
            Player.ToSaveData(),
            Map.CreateSaveData(),
            Event.CreateSaveData()
        );
    }

    /// <summary>
    /// セーブデータから復元
    /// </summary>
    /// <param name="data"></param>
    public void FromSaveData(SaveData data)
    {
        Player.LoadFrom(data.Player);
        Map.RestoreFrom(data.Map);
        Event.RestoreFrom(data.Event);
    }
}