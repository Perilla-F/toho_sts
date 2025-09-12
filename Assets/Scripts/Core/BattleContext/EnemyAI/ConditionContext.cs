public class ConditionContext
{
    public IBattleUnit HeroUnit { get; private set; }
    public int EnemyTurnCount;
    public ConditionContext(IBattleUnit heroUnit)
    {
        this.HeroUnit = heroUnit;
    }
}
