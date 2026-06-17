using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "EnemyAction/NormalAction")]
public class NormalAction : EnemyAction
{
    public EnemyEffect[] effects;
    public string description;
    public EnemyActionType actionType;

    public override async UniTask Execute(IBattleContext context, IEnemyUnit enemy)
    {
        // 対象はターゲット選択などで決定
        BattleUnit target = context.SelectTarget(enemy);
        foreach (var effect in effects)
        {
            await effect.Apply(context, enemy, target);
        }
    }
}