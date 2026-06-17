using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "Effect/Status")]
public class EnemyStatusEffect : EnemyEffect
{
    public StatusEffectData effectData;
    public override async UniTask Apply(IBattleContext context, IEnemyUnit enemy, BattleUnit target)
    {
        target.AddEffect(effectData, amount);
    }
}