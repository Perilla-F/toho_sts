using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite artwork;
    public List<ResourceCost> costs = new();
    public float triggerTime;
    public string description;
    public List<CardEffectData> cardEffects = new();
}