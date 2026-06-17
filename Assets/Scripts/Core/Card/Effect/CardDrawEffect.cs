using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/Draw")]
public class CardDrawEffect : CardEffectDefinition
{
    public override async UniTask Apply(int amount, CardContext context)
    {
        await context.BattleSystem.Draw(amount);
    }
}