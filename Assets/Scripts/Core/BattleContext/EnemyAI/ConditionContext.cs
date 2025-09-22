public class ConditionContext
{
    public BattleUnit HeroUnit { get; private set; }
    public int EnemyTurnCount;
    public ConditionContext(BattleUnit heroUnit)
    {
        this.HeroUnit = heroUnit;
    }
}
