using UnityEngine;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class CardEffectInstance
{
    public CardEffectDefinition Definition;
    public int Amount;
    public CardEffectTarget Target;

    public void Apply(ITargetSelector selector, ICardContext context)
    {
        CardEffectResolver.ResolveEffect(selector, Definition, Amount, context);
    }
}