using UnityEngine;

[System.Serializable]
public class HpBelowCondition : IEnemyCondition
{
    public float threshold;

    public HpBelowCondition(float threshold) => this.threshold = threshold;

    public bool Evaluate(ConditionContext context, IEnemyUnit self)
    {
        return self.GetCurrentHP() < self.GetMaxHP() * threshold;
    }

    public bool IsMet(IEnemyUnit self, int turn, ConditionContext context)
    {
        return self.GetCurrentHP() < self.GetMaxHP() * threshold;
    }

    public string Description => $"敵HPが{threshold * 100}%未満";
}
