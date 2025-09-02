using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SourceCard : ISourceCard
{
    public CardData Data;
    public int SourceCost;
    public int Damage;
    public int DrawAmount;
    public int BlockAmount;
    public int UpgradedLevel;
    public bool IsUpgraded;
    public CardType CardType;

    public SourceCard(CardData data)
    {
        this.Data = data;
        UpgradedLevel = 0;
        IsUpgraded = false;
    }

    public void ModifySourceCost(int delta)
    {
        SourceCost = Mathf.Max(0, SourceCost + delta);
    }
}