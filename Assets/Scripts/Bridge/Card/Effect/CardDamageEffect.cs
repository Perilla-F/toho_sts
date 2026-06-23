using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/Damage")]
public class CardDamageEffect : CardEffectDefinition
{
    public override void Execute(ICardContext context, IBattleUnit target, int amount)
    {
        var finalDamage = context.BattleSystem.CalculateDamage(context.User, target, amount);
        target.TakeDamageAsync(finalDamage);
    }
}