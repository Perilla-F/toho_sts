public interface IBattleDeck : IReadOnlyBattleDeck
{
    public void AddCard(ICardObj card);
    public ICardObj Draw();
    public void Shuffle();
}

public interface IReadOnlyBattleDeck
{
    public int Count { get; }
    public bool IsEmpty();
}