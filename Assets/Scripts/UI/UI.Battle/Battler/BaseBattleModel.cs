using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BaseBattleModel : MonoBehaviour, IAnimatable
{
    public abstract void PlayIdle();
    public abstract void PlayAttack(AnimationClip attack);
    public abstract void PlayHit(AnimationClip hit);
    public abstract UniTask PlayAttackAnimation();
}