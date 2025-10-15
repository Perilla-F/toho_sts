using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SourceCard : ISourceCard
{
    public CardData Data;
    public ResourceRegistry SourceCost;
    public int Delay;
    public int UpgradedLevel;
    public bool IsUpgraded;

    public SourceCard(CardData data, HPResource hPResource, Mana mana)
    {
        Data = data;
        Delay = data.Delay;
        SourceCost = new ResourceRegistry();
        foreach (ResourceCost cost in data.Costs)
        {
            switch (cost.Type)
            {
                case ResourceType.Mana:
                    SourceCost.Register(mana);
                    break;
                case ResourceType.HP:
                    SourceCost.Register(hPResource);
                    break;
            }
        }
        UpgradedLevel = 0;
        IsUpgraded = false;
    }

}