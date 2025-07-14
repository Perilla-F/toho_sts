using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mapUI;
    public GameObject shopUI;
    public GameObject eventUI;

    public void ShowShop()
    {
        HideAll();
        shopUI.SetActive(true);
    }

    public void ShowEvent()
    {
        HideAll();
        eventUI.SetActive(true);
    }

    public void ShowMap()
    {
        HideAll();
        mapUI.SetActive(true);
    }

    private void HideAll()
    {
        mapUI.SetActive(false);
        shopUI.SetActive(false);
        eventUI.SetActive(false);
    }
}
