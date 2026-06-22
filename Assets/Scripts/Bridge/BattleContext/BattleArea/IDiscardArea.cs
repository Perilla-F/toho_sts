public interface IDiscardArea : IReadOnlyDiscardArea
{
    public void AddCard(ICardObj cardObj);
    public void ShuffleBackInto(IBattleDeck battleDeck);
}

public interface IReadOnlyDiscardArea
{
    public int Count { get; }
}