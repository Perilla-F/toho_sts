using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ApplyEffect")]
public class EffectApplyEffect : EnemyEffectData
{
    public StatusEffectData Status;
    public int Amount;
    public IBattleUnit Target;
    public override void Apply(IBattleContext context, IBattleUnit self)
    {
        Target.AddEffect(Status, Amount);
    }
}