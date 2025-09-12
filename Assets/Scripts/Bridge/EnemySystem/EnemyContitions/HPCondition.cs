using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Conditions/HP")]
public class HpCondition : EnemyCondition
{
    public enum Comparison { LessThanOrEqual, GreaterThanOrEqual }
    public Comparison comparison;
    public float hpPercent; // 0〜1で管理

    public override bool IsSatisfied(IBattleContext context, IBattleUnit self)
    {
        float current = (float)self.GetCurrentHP() / self.GetMaxHP();
        return comparison == Comparison.LessThanOrEqual
            ? current <= hpPercent
            : current >= hpPercent;
    }
}