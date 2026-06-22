using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IBattleModel
{
    public void PlayIdle();
    public void PlayAttack(AnimationClip attack);
    public void PlayHit(AnimationClip hit);
    public UniTask PlayAttackAnimation();
}