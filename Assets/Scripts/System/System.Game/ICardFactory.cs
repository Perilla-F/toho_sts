public interface ICardFactory
{
    public CardObj CreateCard(SourceCard sourceCard, IBattleViewRoot view);
}