using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAction/NormalAction")]
public class NormalAction : EnemyAction
{
    public EnemyEffect[] effects;

    public override void Execute(IBattleContext context, EnemyUnit enemy)
    {
        // 対象はターゲット選択などで決定
        BattleUnit target = context.SelectTarget(enemy);
        foreach (var effect in effects)
        {
            effect.Apply(context, enemy, target);
        }
    }
}