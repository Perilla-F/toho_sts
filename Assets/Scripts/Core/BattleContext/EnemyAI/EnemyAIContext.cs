using System.Collections.Generic;

public class EnemyAIContext
{
    public BattleUnit Self { get; private set; }
    public List<BattleUnit> Target { get; private set; }
    public EnemyAIContext(BattleUnit Self, List<BattleUnit> Target)
    {
        this.Self = Self;
        this.Target = Target;
    }
}
