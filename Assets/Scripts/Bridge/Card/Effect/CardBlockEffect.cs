using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/Block")]
public class CardBlockEffect : CardEffectDefinition
{
    public override void Apply(int amount, CardContext context)
    {
        Debug.Log($"Get {amount}Block !");
        context.User.ApplyBlock(amount);
    }
}