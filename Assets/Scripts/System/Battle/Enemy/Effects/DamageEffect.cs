using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Damage")]
public class DamageEffect : EnemyEffect
{
    public override void Apply(BattleContext context, IEnemyUnit enemy, BattleUnit target)
    {
        int damage = amount + enemy.AttackBonus; // enemy のステータスで補正
        target.TakeDamage(damage);
    }
}