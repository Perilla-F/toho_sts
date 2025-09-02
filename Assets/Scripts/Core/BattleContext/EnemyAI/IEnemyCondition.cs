public interface IEnemyCondition
{
    bool Evaluate(ConditionContext context, IEnemyUnit self);
    bool IsMet(IEnemyUnit self, int turn, ConditionContext context);
}