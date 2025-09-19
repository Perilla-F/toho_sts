using System.Collections.Generic;

public class CardContext
{
    private IBattleSystem _battleSystem;
    public IBattleUnit User;
    public List<IBattleUnit> Enemies;
    public ISourceCard SourceCard;
    public List<IBattleUnit> Targets;

    public CardContext(IBattleSystem battleSystem, IBattleUnit user, List<IBattleUnit> enemies, SourceCard source, List<IBattleUnit> targets)
    {
        _battleSystem = battleSystem;
        User = user;
        Enemies = enemies;
        SourceCard = source;
        Targets = targets;
    }

    public IBattleSystem GetBattleSystem()
    {
        return _battleSystem;
    }
}
