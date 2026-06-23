using Cysharp.Threading.Tasks;
using Cysharp.Threading;
using System.Threading;

public interface IBattleSystem
{
    int CalculateDamage(IBattleUnit user, IBattleUnit target, int value);
    UniTask DrawMultipleAsync(int count);
}
