using System.Collections.Generic;

public interface IBattleDeck : IReadOnlyBattleDeck
{
    public void AddCard(ICardObj card);
    public ICardObj Draw();
    public void Shuffle();
}

public interface IReadOnlyBattleDeck
{
    public IReadOnlyList<IReadOnlyCardObj> DeckCards { get; }
    public int Count { get; }
    public bool IsEmpty();
}