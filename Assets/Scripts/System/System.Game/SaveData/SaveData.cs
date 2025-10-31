[System.Serializable]
public class SaveData
{
    public PlayerSaveData Player;
    public MapSaveData Map;
    public EventSaveData Event;

    public SaveData(PlayerSaveData player, MapSaveData map, EventSaveData evt)
    {
        Player = player;
        Event = evt;
        Map = map;
    }
}