using Cysharp.Threading.Tasks;
using System.Threading;

public interface ICardUIHandler
{
    UniTask PlayDrawAnimationAsync(DrawEventData data, CancellationToken ct = default);
    UniTask PlayDiscardAnimationAsync(DiscardEventData data, CancellationToken ct = default);
}