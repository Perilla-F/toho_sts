[System.Serializable]
public class AlwaysTrueCondition : IEnemyCondition
{
    public bool Evaluate(ConditionContext context, IEnemyUnit self) => true;
    public bool IsMet(IEnemyUnit self, int turn, ConditionContext context)
    {
        return true;
    }

    public string Description => "常に実行";
}
