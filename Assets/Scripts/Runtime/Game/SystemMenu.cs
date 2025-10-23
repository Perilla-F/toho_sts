using UnityEngine;

public class SystemMenu : MonoBehaviour
{
    private SaveManager saveManager;

    private void Awake()
    {
        saveManager = ServiceLocator.Get<SaveManager>();
    }

    public void OnSaveButton(SaveData data)
    {
        saveManager.SaveGame(data);
    }

    public void OnLoadButton()
    {
        saveManager.LoadGame();
    }

    public void OnDeleteButton()
    {
        saveManager.DeleteSave();
    }
}
