using System.Threading;
using UnityEngine;
using Live2D.Cubism.Framework.Motion;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class HeroModel : MonoBehaviour, IBattleModel
{
    [SerializeField] private CubismMotionController motionController;
    private AnimationClip idle;

    private CancellationToken _ct;

    public void Init(AnimationClip idle)
    {
        this.idle = idle;
        PlayIdle();
        _ct = this.GetCancellationTokenOnDestroy();
    }

    public void PlayIdle()
    {
        motionController.PlayAnimation(idle, isLoop: true);
    }

    public void PlayAttack(AnimationClip attack)
    {
        motionController.PlayAnimation(attack, isLoop: false);

        // Coroutineでモーション終了後にIdleへ戻す
        float duration = attack.length;
        StartCoroutine(ReturnToIdleAfter(duration));
    }

    public void PlayHit(AnimationClip hit)
    {
        motionController.PlayAnimation(hit, isLoop: false);

        // 被ダメ後にIdleへ戻す
        float duration = hit.length;
        StartCoroutine(ReturnToIdleAfter(duration));
    }

    public async UniTask PlayAttackAnimation()
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