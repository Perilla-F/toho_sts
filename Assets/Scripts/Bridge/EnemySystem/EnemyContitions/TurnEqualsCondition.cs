[System.Serializable]
public class TurnEqualsCondition : IEnemyCondition
{
    private int count;
    public int turnNumber;

    public TurnEqualsCondition(int value) => this.count = value;

    public bool Evaluate(ConditionContext context, IEnemyUnit self)
    {
        return context.EnemyTurnCount <= count;
    }

    public bool IsMet(IEnemyUnit self, int turn, ConditionContext context)
    {
        return turn <= count;
    }

    public string Description => $"{count}ターン目まで実行";
}
