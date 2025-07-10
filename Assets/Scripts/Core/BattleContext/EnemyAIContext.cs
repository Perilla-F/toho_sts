using System.Collections.Generic;

public class EnemyAIContext
{
    public IBattlerUnit Self { get; private set; }
    public List<IBattlerUnit> Target { get; private set; }
    public EnemyAIContext(IBattlerUnit Self, List<IBattlerUnit> Target)
    {
        this.Self = Self;
        this.Target = Target;
    }
}
