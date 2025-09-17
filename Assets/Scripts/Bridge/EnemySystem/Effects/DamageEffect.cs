using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Damage")]
public class DamageEffect : EnemyEffect
{
    public override void Apply(IBattleContext context, EnemyUnit enemy, IBattleUnit target)
    {
        int damage = amount + enemy.AttackBonus; // enemy のステータスで補正
        target.TakeDamage(damage);
    }
}