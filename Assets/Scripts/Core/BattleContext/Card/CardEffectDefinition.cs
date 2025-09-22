using UnityEngine;

public abstract class CardEffectDefinition : ScriptableObject
{
    public abstract void Apply(int amount, CardContext context);
}