using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public string CardName;
    public CardRarity Rarity;
    public Sprite Artwork;
    public List<ResourceCost> Costs;
    public int Delay;
    public int UpgradedLevel;
    public bool IsUpgraded;
    public CardType CardType;
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