public class ConditionContext
{
    public IHeroUnit heroUnit { get; private set; }
    public int EnemyTurnCount;
    public ConditionContext(IHeroUnit heroUnit)
    {
        this.heroUnit = heroUnit;
    }
}
