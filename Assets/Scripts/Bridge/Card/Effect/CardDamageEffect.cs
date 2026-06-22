using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/Damage")]
public class CardDamageEffect : CardEffectDefinition
{
    public override void Apply(int amount, CardContext context)
    {
        foreach (var target in context.Targets)
        {
            int finalDamage = context.BattleSystem.CalculateDamage(context.User, target, amount);
            target.TakeDamageAsync(finalDamage);
        }
    }
}