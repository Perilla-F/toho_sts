using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "Effect/Damage")]
public class DamageEffect : EnemyEffect
{
    public override async UniTask Apply(IBattleContext context, IEnemyUnit enemy, BattleUnit target)
    {
        int damage = amount + enemy.AttackBonus; // enemy のステータスで補正
        await target.TakeDamageAsync(damage);
    }
}