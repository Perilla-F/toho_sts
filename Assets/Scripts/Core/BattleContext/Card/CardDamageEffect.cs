using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/Damage")]
public class CardDamageEffect : CardEffectDefinition
{

    public override void Apply(int amount, CardContext context)
    {
        List<IBattleUnit> Targets = context.Enemies;
        foreach (var target in Targets)
        {
            target.TakeDamage(amount);
        }
    }
}