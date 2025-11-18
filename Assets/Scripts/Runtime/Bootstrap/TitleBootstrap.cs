using UnityEngine;

public class TitleBootstrap : MonoBehaviour
{
    [SerializeField] TitleUI titleUI;

    private TitlePresenter presenter;

    private GameManager gameManager;
    private ISaveService saveService;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        saveService = ServiceLocator.Get<ISaveService>();
        presenter = new TitlePresenter(gameManager, saveService, titleUI);
    }
}