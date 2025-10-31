using UnityEngine;

public class CharacterSelectBootstrap : MonoBehaviour
{
    [SerializeField] CharacterSelectUI characterSelectUI;

    private CharacterSelectPresenter presenter;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        presenter = new CharacterSelectPresenter(gameManager, characterSelectUI);
    }
}