using System.Collections.Generic;

public interface IFlagManager
{
    public bool HasFlag(string flag);
    public void SetFlag(string flag);
    public void RemoveFlag(string flag);
    public List<string> GetAllFlags();
    public void LoadFromSaveData(List<string> loadedFlags);
}