public class CharacterSelectPresenter
{
    private GameManager _game;
    private readonly CharacterSelectUI _UI;

    public CharacterSelectPresenter(GameManager game, CharacterSelectUI ui)
    {
        _game = game;
        _UI = ui;

        // System層 → UI層 通知
        _UI.OnCharacterSelect += OnCharacterSelect;
    }

    // UI層 → System層 命令
    public void OnCharacterSelect(HeroData data)
    {
        _game.SelectCharacter(data);
    }
}
