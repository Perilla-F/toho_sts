using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/Draw")]
public class CardDrawEffect : CardEffectDefinition
{
    public override void Execute(ICardContext context, IBattleUnit target, int amount)
    {
        context.BattleSystem.DrawMultipleAsync(amount);
    }
}