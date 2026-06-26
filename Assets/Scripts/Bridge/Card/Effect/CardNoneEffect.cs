using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/None")]
public class CardNoneEffect : CardEffectDefinition
{
    public override void Execute(ICardContext context, IBattleUnit target, int amount)
    {
    }
}