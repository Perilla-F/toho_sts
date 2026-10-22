using UnityEngine;

public class TitleBootstrap : MonoBehaviour
{
    [SerializeField] TitleUI titleUI;

    private TitlePresenter presenter;

    private IGameManager gameManager;
    private ISaveService saveService;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<IGameManager>();
        saveService = ServiceLocator.Get<ISaveService>();
        presenter = new TitlePresenter(gameManager, saveService, titleUI);
    }
}