using UnityEngine;

public class SystemMenu : MonoBehaviour
{
    public void OnSaveButton()
    {
        SaveManager.Instance.SaveGame();
    }

    public void OnLoadButton()
    {
        SaveManager.Instance.LoadGame();
    }

    public void OnDeleteButton()
    {
        SaveManager.Instance.DeleteSave();
    }
}
