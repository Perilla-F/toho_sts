using Cysharp.Threading.Tasks;

public interface ICardEffect
{
    UniTask ResolveAsync(CardContext context);
}
