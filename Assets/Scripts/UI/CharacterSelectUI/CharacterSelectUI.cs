using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private HeroDatabase heroDatabase;
    private HeroData _reimu;
    private HeroData _marisa;

    private IGameManager gameManager;

    public void Init(IGameManager gameManager)
    {
        this.gameManager = gameManager;
        _reimu = heroDatabase.GetHeroData("reimu");
        _marisa = heroDatabase.GetHeroData("marisa");
    }

    public void OnSelectReimu()
    {
        gameManager.InitializePlayer(_reimu);
        SceneManager.LoadScene("MapScene");
    }

    public void OnSelectMarisa()
    {
        gameManager.InitializePlayer(_marisa);
        SceneManager.LoadScene("MapScene");
    }
}
