using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/Damage")]
public class CardDamageEffect : CardEffectDefinition
{
    public override void Apply(int amount, CardContext context)
    {
        List<BattleUnit> Targets = context.Targets;
        foreach (var target in Targets)
        {
            target.TakeDamage(amount);
        }
    }
}