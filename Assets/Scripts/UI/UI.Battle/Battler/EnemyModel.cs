using UnityEngine;
using UnityEngine.EventSystems;
using Live2D.Cubism.Framework.Motion;

public class EnemyModel : MonoBehaviour, IBattleModel, IDropHandler
{
    [SerializeField] private CubismMotionController motionController;
    private EnemyUnit _self;

    public void Init(EnemyUnit enemy)
    {
        _self = enemy;
        PlayIdle();
    }

    public void PlayIdle()
    {
        if (_self.IdleClip != null)
            motionController.PlayAnimation(_self.IdleClip, isLoop: true);
    }

    public void PlayAttack()
    {
        if (_self.AttackClip != null)
            motionController.PlayAnimation(_self.AttackClip, isLoop: false);

        // Coroutineでモーション終了後にIdleへ戻す
        float duration = _self.AttackClip.length;
        StartCoroutine(ReturnToIdleAfter(duration));
    }

    public void PlayHit()
    {
        if (_self.HitClip != null)
            motionController.PlayAnimation(_self.HitClip, isLoop: false);

        // 被ダメ後にIdleへ戻す
        float duration = _self.HitClip.length;
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
        CardObj card = eventData.pointerDrag.GetComponent<CardObj>();
    }
}