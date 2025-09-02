[System.Serializable]
public class PlayerHasStatusCondition : IEnemyCondition
{
    public string StatusName;

    public PlayerHasStatusCondition(string status) => this.StatusName = status;

    public bool Evaluate(ConditionContext context, IEnemyUnit self)
    {
        return context.HeroUnit.HasStatus(StatusName);
    }
    public bool IsMet(IEnemyUnit self, int turn, ConditionContext context)
    {
        return context.HeroUnit.HasStatus(StatusName);
    }

    public string Description => $"プレイヤーが状態「{StatusName}」を持つ";
}
