using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public string CardName;
    public Sprite Artwork;
    public List<ResourceCost> Costs = new();
    public float TriggerTime;
    public string Description;
    public List<CardEffectData> CardEffects = new();
}