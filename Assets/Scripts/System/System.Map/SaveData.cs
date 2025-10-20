using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public HPResource HPResource;
    public int gold;
    public List<string> Flags = new List<string>();
    public MapSaveData Map;
    public LastEventData LastEvent;
}
