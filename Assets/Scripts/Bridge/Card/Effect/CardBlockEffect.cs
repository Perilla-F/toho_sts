using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/Block")]
public class CardBlockEffect : CardEffectDefinition
{
    public override void Execute(ICardContext context, IBattleUnit target, int amount)
    {
        Debug.Log($"Get {amount}Block !");
        target.ApplyBlock(amount);
        BattleEventBus.View.OnUpdateHp(target);
    }
}