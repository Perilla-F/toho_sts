using System.Collections.Generic;

public class CardContext
{
    public IBattleSystem BattleSystem;
    public IBattleUnit User;
    public List<IBattleUnit> Targets;

    public CardContext(IBattleSystem battleSystem, IBattleUnit user, List<IBattleUnit> targets)
    {
        BattleSystem = battleSystem;
        User = user;
        Targets = targets;
    }
}
