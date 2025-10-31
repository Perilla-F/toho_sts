[System.Serializable]
public class SourceCard
{
    public CardData Data;
    public ResourceRegistry SourceCost;

    public SourceCard(CardData data, HPResource hPResource, Mana mana)
    {
        Data = data;
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
    }

}