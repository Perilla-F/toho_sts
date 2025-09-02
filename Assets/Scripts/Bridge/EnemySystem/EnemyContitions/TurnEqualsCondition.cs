[System.Serializable]
public class TurnEqualsCondition : IEnemyCondition
{
    private int Count;
    public int TurnNumber;

    public TurnEqualsCondition(int value) => this.Count = value;

    public bool Evaluate(ConditionContext context, IEnemyUnit self)
    {
        return context.EnemyTurnCount <= Count;
    }

    public bool IsMet(IEnemyUnit self, int turn, ConditionContext context)
    {
        return turn <= Count;
    }

    public string Description => $"{Count}ターン目まで実行";
}
