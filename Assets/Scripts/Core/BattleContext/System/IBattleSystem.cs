using Cysharp.Threading.Tasks;
using Cysharp.Threading;
using System.Threading;

public interface IBattleSystem
{
    UniTask Draw(int count);
}
