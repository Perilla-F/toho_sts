using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MapUI;
    public GameObject ShopUI;
    public GameObject EventUI;

    public void ShowShop()
    {
        HideAll();
        ShopUI.SetActive(true);
    }

    public void ShowEvent()
    {
        HideAll();
        EventUI.SetActive(true);
    }

    public void ShowMap()
    {
        HideAll();
        MapUI.SetActive(true);
    }

    private void HideAll()
    {
        MapUI.SetActive(false);
        ShopUI.SetActive(false);
        EventUI.SetActive(false);
    }
}
