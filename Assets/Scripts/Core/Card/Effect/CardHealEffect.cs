using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/Heal")]
public class CardHealEffect : CardEffectDefinition
{
    public override async UniTask Apply(int amount, CardContext context)
    {
        context.User.Heal(amount);
    }
}