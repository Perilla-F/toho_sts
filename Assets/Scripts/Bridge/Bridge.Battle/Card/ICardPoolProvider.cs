// Bridge層
public interface ICardPoolProvider
{
    IPoolableCard GetCard();
    void ReturnCard(IPoolableCard card);
}