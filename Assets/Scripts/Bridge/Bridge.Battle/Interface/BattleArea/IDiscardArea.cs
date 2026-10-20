using System.Collections.Generic;

public interface IDiscardArea : IReadOnlyDiscardArea
{
    public void AddCard(ICardObj cardObj);
    public void ShuffleBackInto(IBattleDeck battleDeck);
}

public interface IReadOnlyDiscardArea
{
    public IReadOnlyList<IReadOnlyCardObj> DiscardedCards { get; }
    public int Count { get; }
}