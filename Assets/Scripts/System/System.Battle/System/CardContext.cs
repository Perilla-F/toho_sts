using System.Collections.Generic;

public class CardContext : ICardContext
{
    private IBattleSystem _battleSystem;
    IBattleSystem ICardContext.BattleSystem => _battleSystem;
    private IBattleUnit _user;
    IBattleUnit ICardContext.User => _user;
    IReadOnlyBattleUnit IReadOnlyCardContext.User => _user;
    private List<IBattleUnit> _targets;
    List<IBattleUnit> ICardContext.Targets => _targets;
    IReadOnlyList<IReadOnlyBattleUnit> IReadOnlyCardContext.Targets => _targets;

    public CardContext(IBattleSystem battleSystem, IBattleUnit user, List<IBattleUnit> targets)
    {
        _battleSystem = battleSystem;
        _user = user;
        _targets = targets;
    }
}
