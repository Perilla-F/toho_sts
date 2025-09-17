using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/Draw")]
public class CardDrawEffect : CardEffectDefinition
{
    public override void Apply(int amount, CardContext context)
    {
        context.BattleSystem.Draw(amount);
    }
}