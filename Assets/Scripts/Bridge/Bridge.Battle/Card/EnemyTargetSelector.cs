using System.Collections.Generic;

public class EnemyTargetSelector : ITargetSelector
{
    public IEnumerable<IBattleUnit> GetTargets(ICardContext context) => new[] { context.Targets[0] };
}