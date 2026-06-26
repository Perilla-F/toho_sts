using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Live2D.Cubism.Framework.Motion;
using Live2D.Cubism.Framework.MotionFade;

public class EnemyModel : BaseBattleModel
{
    public IReadOnlyEnemyUnit Self;
    private AnimationClip idle;

    private CancellationToken _ct;
    private CubismMotionController _motionController;
    private CubismFadeMotionList _motionList;

    public void Initialize(IReadOnlyEnemyUnit enemy, EnemyData data, CubismMotionController motionController, CubismFadeMotionList motionList)
    {
        Self = enemy;
        idle = data.IdleClip;
        _motionController = motionController;
        _motionList = motionList;
        PlayIdle();

        _ct = this.GetCancellationTokenOnDestroy();
    }

    public override void PlayIdle()
    {
        if (_motionController != null)
        {
            _motionController.PlayAnimation(idle, isLoop: true);
        }
    }

    public override void PlayAttack(AnimationClip attack)
    {
        _motionController.PlayAnimation(attack, isLoop: false);

        // Coroutineでモーション終了後にIdleへ戻す
        float duration = attack.length;
        StartCoroutine(ReturnToIdleAfter(duration));
    }

    public override async UniTask PlayAttackAnimation()
    {
        // 前に飛び出す -> 戻る
        var originalPos = transform.localPosition;
        await transform.DOLocalMove(originalPos + Vector3.left * 50, 0.2f)
            .SetEase(Ease.OutQuad)
            .WithCancellation(_ct);

        await transform.DOLocalMove(originalPos, 0.1f)
            .SetEase(Ease.InQuad)
            .WithCancellation(_ct);
    }

    public override void PlayHit(AnimationClip hit)
    {
        _motionController.PlayAnimation(hit, isLoop: false);

        // 被ダメ後にIdleへ戻す
        float duration = hit.length;
        StartCoroutine(ReturnToIdleAfter(duration));
    }

    private System.Collections.IEnumerator ReturnToIdleAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayIdle();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop!");
        BattleCard card = eventData.pointerDrag.GetComponent<BattleCard>();
    }

}