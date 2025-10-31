using System.Collections.Generic;

public class CardContext
{
    private IBattleSystem _battleSystem;
    public BattleUnit User;
    public List<BattleUnit> Enemies;
    public SourceCard SourceCard;
    public List<BattleUnit> Targets;

    public CardContext(IBattleSystem battleSystem, BattleUnit user, List<BattleUnit> enemies, SourceCard source, List<BattleUnit> targets)
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
