using System.Collections.Generic;

public interface IGameManager
{
    public List<SourceCard> GetSourceCards();
    public void SelectCharacter(HeroData data);
    public void StartBattle(EnemyType type);
    public void LoadGame();
    public void RequestSave();
}