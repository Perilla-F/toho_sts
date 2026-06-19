using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/Block")]
public class CardBlockEffect : CardEffectDefinition
{
    public override async UniTask Apply(int amount, CardContext context)
    {
        await context.User.ApplyBlock(amount);
    }
}