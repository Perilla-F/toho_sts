using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "EnemyAction/NormalAction")]
public class NormalAction : EnemyAction
{
    public List<EnemyEffect> effects;
    public override async UniTask Execute(int id, IBattleContext context)
    {
        // 対象はターゲット選択などで決定
        foreach (var effect in effects)
        {
            var targets = SelectTarget(id, context, effect);
            foreach (var target in targets)
            {
                EnemyEffectResolver.ResolveEffect(id, context, effect, target);
            }
        }
        // 演出の開始を待機する準備
        var tcs = new UniTaskCompletionSource();

        // 演出発火（通知）
        BattleEventBus.BattleEventAsync.OnEnemyAttackEffect?.Invoke(id, tcs);

        // 演出が終わるまで待つ
        await tcs.Task;
    }

    private List<IBattleUnit> SelectTarget(int id, IBattleContext context, EnemyEffect effect)
    {
        var targets = new List<IBattleUnit>();
        switch (effect.Target)
        {
            case EnemyActionTarget.Hero:
                targets.Add(context.Hero);
                break;
            case EnemyActionTarget.Self:
                var Self = context.Enemies.GetEnemy(id);
                targets.Add(Self);
                break;
            case EnemyActionTarget.Member:
                targets.Add(context.Enemies.GetRandomAliveEnemy());
                break;
            case EnemyActionTarget.Group:
                targets.AddRange(context.Enemies.GetAllEnemies());
                break;
            default:
                targets.Add(context.Hero);
                break;
        }
        return targets;
    }

}