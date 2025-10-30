using UnityEngine;
using UnityEngine.EventSystems;
using Live2D.Cubism.Framework.Motion;

public class EnemyModel : MonoBehaviour, IBattleModel, IDropHandler
{
    [SerializeField] private CubismMotionController motionController;
    private BattleUnit _self;

    private AnimationClip idle;

    public void Init(BattleUnit enemy, AnimationClip idle)
    {
        _self = enemy;
        this.idle = idle;
        PlayIdle();
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

    private System.Collections.IEnumerator ReturnToIdleAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayIdle();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop!");
        CardBehavior card = eventData.pointerDrag.GetComponent<CardBehavior>();
    }

}