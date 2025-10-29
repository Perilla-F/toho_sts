using UnityEngine;
using Live2D.Cubism.Framework.Motion;

public class HeroModel : MonoBehaviour, IBattleModel
{
    [SerializeField] private CubismMotionController motionController;
    private AnimationClip idle;

    public void Init(AnimationClip idle)
    {
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

}