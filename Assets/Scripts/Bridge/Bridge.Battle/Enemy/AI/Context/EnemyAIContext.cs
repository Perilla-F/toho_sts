using System.Collections.Generic;

public class EnemyAIContext
{
    public IBattleUnit Self { get; private set; }
    public List<IBattleUnit> Target { get; private set; }
    public EnemyAIContext(IBattleUnit Self, List<IBattleUnit> Target)
    {
        this.Self = Self;
        this.Target = Target;
    }
}
