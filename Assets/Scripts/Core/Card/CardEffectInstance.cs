using UnityEngine;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class CardEffectInstance
{
    public CardEffectDefinition Definition;
    public int Amount;

    public async UniTask Apply(CardContext context)
    {
        await Definition.Apply(Amount, context);
    }
}