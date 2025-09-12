using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Conditions/Status")]
public class StatusCondition : EnemyCondition
{
    public enum Target { Self, Player }
    public Target target;
    public StatusEffectData status;

    public override bool IsSatisfied(IBattleContext context, IBattleUnit self)
    {
        var entity = target == Target.Self ? self : context.Hero;
        return entity.HasStatus(status);
    }
}