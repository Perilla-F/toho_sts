using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public string CardName;
    public Sprite Artwork;
    public List<ResourceCost> Costs;
    public CardType CardType;
    public int Delay;
    public string Description;
    public List<CardEffectInstance> CardEffects;
    public CardEffectTarget CardEffectTarget;

    public void ApplyEffects(CardContext context)
    {
        foreach (var effect in CardEffects)
        {
            effect.Apply(context);
        }
    }
}