using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/Heal")]
public class CardHealEffect : CardEffectDefinition
{
    public override void Apply(int amount, CardContext context)
    {
        context.User.Heal(amount);
    }
}