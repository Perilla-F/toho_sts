using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour, IFlagManager
{
    private HashSet<string> activeFlags = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// フラグを持っているか
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public bool HasFlag(string flag)
    {
        return activeFlags.Contains(flag);
    }

    /// <summary>
    /// フラグをセットする
    /// </summary>
    /// <param name="flag"></param>
    public void SetFlag(string flag)
    {
        if (!activeFlags.Contains(flag))
        {
            activeFlags.Add(flag);
            Debug.Log($"Flag ON: {flag}");
        }
    }

    /// <summary>
    /// フラグを消す
    /// </summary>
    /// <param name="flag"></param>
    public void RemoveFlag(string flag)
    {
        if (activeFlags.Remove(flag))
        {
            Debug.Log($"Flag OFF: {flag}");
        }
    }

    /// <summary>
    /// フラグをすべて取り出す
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllFlags()
    {
        return new List<string>(activeFlags);
    }

    /// <summary>
    /// ロード時にフラグを呼び出す
    /// </summary>
    /// <param name="loadedFlags"></param>
    public void LoadFromSaveData(List<string> loadedFlags)
    {
        activeFlags = new HashSet<string>(loadedFlags);
    }
}
