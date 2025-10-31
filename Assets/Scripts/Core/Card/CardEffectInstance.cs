using UnityEngine;

[System.Serializable]
public class CardEffectInstance
{
    public CardEffectDefinition Definition;
    public int Amount;

    public void Apply(CardContext context)
    {
        Definition.Apply(Amount, context);
    }
}