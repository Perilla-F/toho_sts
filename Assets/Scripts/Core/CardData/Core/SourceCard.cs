using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SourceCard : ISourceCard
{
    public CardData data;
    public int sourceCost;
    public int damage;
    public int drawAmount;
    public int blockAmount;
    public int upgradedLevel;
    public bool isUpgraded;
    public CardType cardType;

    public SourceCard(CardData data)
    {
        this.data = data;
        upgradedLevel = 0;
        isUpgraded = false;
    }

    public void ModifySourceCost(int delta)
    {
        sourceCost = Mathf.Max(0, sourceCost + delta);
    }
}