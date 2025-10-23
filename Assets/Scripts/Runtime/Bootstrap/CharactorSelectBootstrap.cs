using UnityEngine;

public class CharacterSelectBootstrap : MonoBehaviour
{
    [SerializeField] CharacterSelectUI characterSelectUI;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        characterSelectUI.Init(gameManager);
    }
}