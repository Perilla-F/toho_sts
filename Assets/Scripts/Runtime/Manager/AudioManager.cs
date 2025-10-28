using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour, IAudioManager
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> bgmClips = new();
    [SerializeField] private List<AudioClip> seClips = new();

    private Dictionary<string, AudioClip> bgmDict = new();
    private Dictionary<string, AudioClip> seDict = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // AudioClipを辞書に登録
        foreach (var clip in bgmClips)
        {
            bgmDict[clip.name] = clip;
        }

        foreach (var clip in seClips)
        {
            seDict[clip.name] = clip;
        }
    }

    // --- BGM 再生 ---
    public void PlayBGM(string name, bool loop)
    {
        if (!bgmDict.TryGetValue(name, out var clip))
        {
            Debug.LogWarning($"BGM '{name}' が見つかりません。");
            return;
        }

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    // --- BGM 停止 ---
    public void StopBGM()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    // --- SE 再生 ---
    public void PlaySE(string name)
    {
        if (!seDict.TryGetValue(name, out var clip))
        {
            Debug.LogWarning($"SE '{name}' が見つかりません。");
            return;
        }

        seSource.PlayOneShot(clip);
    }

    // --- 音量調整 ---
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSEVolume(float volume)
    {
        seSource.volume = Mathf.Clamp01(volume);
    }

    // --- フェード機能（任意） ---
    public void FadeOutBGM(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = bgmSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        StopBGM();
        bgmSource.volume = startVolume;
    }
}
