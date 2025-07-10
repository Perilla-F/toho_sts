using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private HeroData reimu;
    [SerializeField] private HeroData marisa;

    public void OnSelectReimu()
    {
        GameManager.Instance.selectedHeroData = reimu;
        GameManager.Instance.InitializePlayer();
        SceneManager.LoadScene("BattleScene");
    }

    public void OnSelectMarisa()
    {
        GameManager.Instance.selectedHeroData = marisa;
        GameManager.Instance.InitializePlayer();
        SceneManager.LoadScene("BattleScene");
    }
}
