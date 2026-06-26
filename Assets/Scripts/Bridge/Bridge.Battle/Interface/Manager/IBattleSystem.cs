using Cysharp.Threading.Tasks;
using Cysharp.Threading;
using System.Threading;

public interface IBattleSystem
{
    public void ExecuteAttack(IBattleUnit user, IBattleUnit target, int baseDamage);
    UniTask DrawMultipleAsync(int count);
}
