using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/Block")]
public class CardBlockEffect : CardEffectDefinition
{
    public override async UniTask Apply(int amount, CardContext context)
    {
        context.User.ApplyBlock(amount);
    }
}