using System.Threading;
using UnityEngine;
using Live2D.Cubism.Framework.Motion;
using Live2D.Cubism.Framework.MotionFade;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class HeroModel : BaseBattleModel
{
    private CubismMotionController _motionController;
    private AnimationClip idle;

    private CancellationToken _ct;

    public void Initialize(HeroData data, CubismMotionController controller)
    {
        idle = data.IdleClip;
        _motionController = controller;
        PlayIdle();
        _ct = this.GetCancellationTokenOnDestroy();
    }

    public override void PlayIdle()
    {
        _motionController.PlayAnimation(idle, isLoop: true);
    }

    public override void PlayAttack(AnimationClip attack)
    {
        _motionController.PlayAnimation(attack, isLoop: false);

        // Coroutineでモーション終了後にIdleへ戻す
        float duration = attack.length;
        StartCoroutine(ReturnToIdleAfter(duration));
    }

    public override void PlayHit(AnimationClip hit)
    {
        _motionController.PlayAnimation(hit, isLoop: false);

        // 被ダメ後にIdleへ戻す
        float duration = hit.length;
        StartCoroutine(ReturnToIdleAfter(duration));
    }

    public override async UniTask PlayAttackAnimation()
    {
        // 前に飛び出す -> 戻る
        var originalPos = transform.localPosition;
        await transform.DOLocalMove(originalPos + Vector3.right * 50, 0.2f)
            .SetEase(Ease.OutQuad)
            .WithCancellation(_ct);

        await transform.DOLocalMove(originalPos, 0.1f)
            .SetEase(Ease.InQuad)
            .WithCancellation(_ct);
    }

    private System.Collections.IEnumerator ReturnToIdleAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayIdle();
    }

}