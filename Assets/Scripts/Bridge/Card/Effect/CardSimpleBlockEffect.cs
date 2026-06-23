using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/SimpleBlock")]
public class CardSimpleBlockEffect : CardEffectDefinition
{
    public override void Execute(ICardContext context, IBattleUnit target, int amount)
    {
        target.ApplySimpleBlock(amount);
    }
}