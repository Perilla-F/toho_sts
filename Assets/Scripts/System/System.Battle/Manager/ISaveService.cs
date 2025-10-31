public interface ISaveService
{
    public void SaveGame(SaveData data);
    public SaveData LoadGame();
    public bool HasSaveData();
    public void DeleteSave();
}