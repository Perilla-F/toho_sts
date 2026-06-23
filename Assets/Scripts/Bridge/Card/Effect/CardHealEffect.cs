using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/Heal")]
public class CardHealEffect : CardEffectDefinition
{
    public override void Execute(ICardContext context, IBattleUnit target, int amount)
    {
        target.Heal(amount);
    }
}