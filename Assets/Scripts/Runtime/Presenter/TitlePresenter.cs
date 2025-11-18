public class TitlePresenter
{
    private GameManager _game;
    private ISaveService _save;
    private readonly TitleUI _ui;

    public TitlePresenter(GameManager game, ISaveService save, TitleUI ui)
    {
        _game = game;
        _ui = ui;

        _ui.Setup(save.HasSaveData());

        // System層 → UI層 通知
        _ui.OnContinue += OnContinue;
    }

    // UI層 → System層 命令
    public void OnContinue()
    {
        _game.LoadGame();
    }
}