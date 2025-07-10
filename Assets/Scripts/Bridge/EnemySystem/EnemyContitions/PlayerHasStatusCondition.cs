[System.Serializable]
public class PlayerHasStatusCondition : IEnemyCondition
{
    public string statusName;

    public PlayerHasStatusCondition(string status) => this.statusName = status;

    public bool Evaluate(ConditionContext context, IEnemyUnit self)
    {
        return context.heroUnit.HasStatus(statusName);
    }
    public bool IsMet(IEnemyUnit self, int turn, ConditionContext context)
    {
        return context.heroUnit.HasStatus(statusName);
    }

    public string Description => $"プレイヤーが状態「{statusName}」を持つ";
}
