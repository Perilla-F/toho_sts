using UnityEngine;

public class CharacterSelectBootstrap : MonoBehaviour
{
    [SerializeField] CharacterSelectUI characterSelectUI;

    private CharacterSelectPresenter presenter;

    private IGameManager gameManager;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<IGameManager>();
        presenter = new CharacterSelectPresenter(gameManager, characterSelectUI);
    }
}