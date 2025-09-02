public class ConditionContext
{
    public IHeroUnit HeroUnit { get; private set; }
    public int EnemyTurnCount;
    public ConditionContext(IHeroUnit heroUnit)
    {
        this.HeroUnit = heroUnit;
    }
}
