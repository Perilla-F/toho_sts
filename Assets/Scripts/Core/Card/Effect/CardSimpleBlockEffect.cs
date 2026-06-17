using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/SimpleBlock")]
public class CardSimpleBlockEffect : CardEffectDefinition
{
    public CardEffectTarget TargetType => CardEffectTarget.Self;
    public CardEffectType EffectType => CardEffectType.SimpleBlock;

    public override async UniTask Apply(int amount, CardContext context)
    {
        context.User.ApplySimpleBlock(amount);
    }
}