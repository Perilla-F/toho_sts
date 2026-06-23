using System.Collections.Generic;

public interface IHand : IReadOnlyHand
{
    new List<ICardObj> Cards { get; }
    public void AddCard(ICardObj card);
    public void RemoveCard(ICardObj cardObj);
}

public interface IReadOnlyHand
{
    public IReadOnlyList<IReadOnlyCardObj> Cards { get; }
}