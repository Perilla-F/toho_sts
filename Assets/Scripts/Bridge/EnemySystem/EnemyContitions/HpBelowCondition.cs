using UnityEngine;

[System.Serializable]
public class HpBelowCondition : IEnemyCondition
{
    public float Threshold;

    public HpBelowCondition(float threshold) => this.Threshold = threshold;

    public bool Evaluate(ConditionContext context, IEnemyUnit self)
    {
        return self.GetCurrentHP() < self.GetMaxHP() * Threshold;
    }

    public bool IsMet(IEnemyUnit self, int turn, ConditionContext context)
    {
        return self.GetCurrentHP() < self.GetMaxHP() * Threshold;
    }

    public string Description => $"敵HPが{Threshold * 100}%未満";
}
