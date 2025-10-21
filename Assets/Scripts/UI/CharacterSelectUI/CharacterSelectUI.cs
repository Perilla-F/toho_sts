using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private HeroData _reimu;
    [SerializeField] private HeroData _marisa;

    public void OnSelectReimu()
    {
        GameManager.Instance.SelectedHeroData = _reimu;
        GameManager.Instance.InitializePlayer();
        SceneManager.LoadScene("MapScene");
    }

    public void OnSelectMarisa()
    {
        GameManager.Instance.SelectedHeroData = _marisa;
        GameManager.Instance.InitializePlayer();
        SceneManager.LoadScene("MapScene");
    }
}
