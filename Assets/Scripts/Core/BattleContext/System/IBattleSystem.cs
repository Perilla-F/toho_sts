using Cysharp.Threading.Tasks;
using Cysharp.Threading;
using System.Threading;

public interface IBattleSystem
{
    int CalculateDamage(BattleUnit user, BattleUnit target, int value);
    UniTask Draw(int count);
}
