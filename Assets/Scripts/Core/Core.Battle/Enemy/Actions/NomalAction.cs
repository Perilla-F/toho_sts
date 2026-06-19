using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "EnemyAction/NormalAction")]
public class NormalAction : EnemyAction
{
    public List<EnemyEffect> effects;
    public EnemyActionType actionType;

    public override async UniTask Execute(IBattleContext context, IEnemyUnit enemy)
    {
        // 対象はターゲット選択などで決定
        BattleUnit target = context.SelectTarget(enemy);
        foreach (var effect in effects)
        {
            await EffectResolver.ResolveEffect(effect, target);
        }
    }
}