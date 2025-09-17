using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/Block")]
public class CardBlockEffect : CardEffectDefinition
{
    public override void Apply(int amount, CardContext context)
    {
        context.User.ApplyBlock(amount);
    }
}