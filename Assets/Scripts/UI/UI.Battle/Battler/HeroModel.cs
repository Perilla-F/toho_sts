using UnityEngine;
using Live2D.Cubism.Framework.Motion;

public class HeroModel : MonoBehaviour, IBattleModel
{
    [SerializeField] private CubismMotionController motionController;
    private HeroUnit _hero;

    public void Init(HeroUnit hero)
    {
        _hero = hero;
        PlayIdle();
    }

    public void PlayIdle()
    {
        if (_hero.IdleClip != null)
            motionController.PlayAnimation(_hero.IdleClip, isLoop: true);
    }

    public void PlayAttack()
    {
        if (_hero.AttackClip != null)
            motionController.PlayAnimation(_hero.AttackClip, isLoop: false);

        // Coroutineでモーション終了後にIdleへ戻す
        float duration = _hero.AttackClip.length;
        StartCoroutine(ReturnToIdleAfter(duration));
    }

    public void PlayHit()
    {
        if (_hero.HitClip != null)
            motionController.PlayAnimation(_hero.HitClip, isLoop: false);

        // 被ダメ後にIdleへ戻す
        float duration = _hero.HitClip.length;
        StartCoroutine(ReturnToIdleAfter(duration));
    }

    private System.Collections.IEnumerator ReturnToIdleAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayIdle();
    }

}