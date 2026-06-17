using System.Collections.Generic;

public class CardContext
{
    public IBattleSystem BattleSystem;
    public BattleUnit User;
    public List<BattleUnit> Targets;

    public CardContext(IBattleSystem battleSystem, BattleUnit user, List<BattleUnit> targets)
    {
        BattleSystem = battleSystem;
        User = user;
        Targets = targets;
    }
}
