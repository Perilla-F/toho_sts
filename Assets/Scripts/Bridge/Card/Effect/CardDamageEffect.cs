using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/Damage")]
public class CardDamageEffect : CardEffectDefinition
{
    public override void Execute(ICardContext context, IBattleUnit target, int amount)
    {
        context.BattleSystem.ExecuteAttack(context.User, target, amount);
    }
}