using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class EnemyEffect : ScriptableObject
{
    public string effectName;
    public int amount;
    public abstract UniTask Apply(IBattleContext context, IEnemyUnit enemy, BattleUnit target);
}