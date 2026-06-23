using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/Status")]
public class CardApplyStatusEffect : CardEffectDefinition
{
    public EffectData Effect;

    public override void Execute(ICardContext context, IBattleUnit target, int amount)
    {
        target.AddEffect(Effect, amount);
    }
}