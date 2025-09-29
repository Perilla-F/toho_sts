using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 戦闘遷移用フェード
/// </summary>
public class BattleTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeGroup; // Canvasに置いたフルスクリーン黒Imageにアタッチ

    private void Awake()
    {
        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// マップ → 戦闘遷移
    /// </summary>
    public void StartBattleTransition(string battleSceneName, BattleTransitionData data, float fadeDuration = 1f)
    {
        StartCoroutine(FadeOutAndLoad(battleSceneName, data, fadeDuration));
    }

    private IEnumerator FadeOutAndLoad(string sceneName, BattleTransitionData data, float duration)
    {
        if (fadeGroup == null)
        {
            SceneLoader.Instance.SetTransitionData(data);
            SceneLoader.Instance.LoadScene(sceneName);
            yield break;
        }

        fadeGroup.DOKill();
        fadeGroup.alpha = 0f;
        fadeGroup.gameObject.SetActive(true);

        bool completed = false;
        fadeGroup.DOFade(1f, duration).OnComplete(() => completed = true);

        // フェード完了まで待機
        yield return new WaitUntil(() => completed);

        // 遷移データを SceneLoader にセットしてロード
        SceneLoader.Instance.SetTransitionData(data);
        SceneLoader.Instance.LoadScene(sceneName);
    }

    /// <summary>
    /// 戦闘後マップ復帰時のフェードイン
    /// </summary>
    public void FadeIn(float duration = 1f)
    {
        if (fadeGroup == null) return;

        fadeGroup.DOKill();
        fadeGroup.alpha = 1f;
        fadeGroup.gameObject.SetActive(true);

        fadeGroup.DOFade(0f, duration).OnComplete(() =>
        {
            fadeGroup.gameObject.SetActive(false);
        });
    }
}
