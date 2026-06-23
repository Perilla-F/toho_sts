using System.Collections.Generic;

public interface ITargetSelector
{
    IEnumerable<IBattleUnit> GetTargets(ICardContext context);
}