using Cysharp.Threading.Tasks;

public interface ICardView
{
    public UniTask MoveToHandAsync();
    public UniTask MoveToDiscardAsync();
}