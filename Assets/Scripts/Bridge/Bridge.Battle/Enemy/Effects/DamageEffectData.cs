using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Damage")]
public class DamageEffectData : EffectData
{
    public override void Apply(int id, IBattleContext context, IBattleUnit target, int amount)
    {
        var self = context.Enemies.GetEnemy(id);
        context.BattleSystem.ExecuteAttack(self, target, amount);
    }
}