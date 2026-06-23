using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/HPBellow")]
public class HPBelowCondition : Condition
{
    public float threshold; // 0~1 の割合

    public override bool Check(IReadOnlyBattleContext context, IReadOnlyEnemyUnit enemy)
    {
        return (float)enemy.HPResource.GetHP() / enemy.HPResource.MaxHP <= threshold;
    }
}