using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Condition/HPBellow")]
public class HPBelowCondition : Condition
{
    public float threshold; // 0~1 の割合

    public override bool Check(BattleContext context, EnemyUnit enemy)
    {
        return (float)enemy.HPResource.GetHP() / enemy.HPResource.MaxHP <= threshold;
    }
}