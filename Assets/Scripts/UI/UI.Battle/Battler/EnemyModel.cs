using UnityEngine;
using UnityEngine.EventSystems;
using Live2D.Cubism.Framework.Motion;

public class EnemyModel : MonoBehaviour, IBattleModel, IDropHandler
{
    private BattleUnit _self;

    private AnimationClip idle;

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

    public void Init(BattleUnit enemy, AnimationClip idle)
    {
        _self = enemy;
        this.idle = idle;
        PlayIdle();
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