using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Live2D.Cubism.Framework.Motion;

public class EnemyModel : MonoBehaviour, IBattleModel, IDropHandler
{
    public IBattleUnit Self;
    private AnimationClip idle;


    private CancellationToken _ct;

    private CubismMotionController MotionController
    {
        get
        {
            if (_motionController == null)
            {
                _motionController = GetComponent<CubismMotionController>();
            }
            return _motionController;
        }
    }
    private CubismMotionController _motionController;

    private void Start() { }

    public void Init(IBattleUnit enemy, AnimationClip idle)
    {
        Self = enemy;
        this.idle = idle;
        PlayIdle();

        _ct = this.GetCancellationTokenOnDestroy();
    }


    public void PlayIdle()
    {
        if (MotionController != null)
        {
            MotionController.PlayAnimation(idle, isLoop: true);
        }
    }

    public void PlayAttack(AnimationClip attack)
    {
        MotionController.PlayAnimation(attack, isLoop: false);

        // Coroutineでモーション終了後にIdleへ戻す
        float duration = attack.length;
        StartCoroutine(ReturnToIdleAfter(duration));
    }

    public async UniTask PlayAttackAnimation()
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

    public void PlayHit(AnimationClip hit)
    {
        MotionController.PlayAnimation(hit, isLoop: false);

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