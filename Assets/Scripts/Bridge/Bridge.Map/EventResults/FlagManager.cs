using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance { get; private set; }
    private HashSet<string> flags = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// フラグを立てる
    /// </summary>
    /// <param name="flagName"></param>
    public void SetFlag(string flagName) => flags.Add(flagName);

    /// <summary>
    /// フラグを消す
    /// </summary>
    /// <param name="flagName"></param>

    public void RemoveFlag(string flagName) => flags.Remove(flagName);

    /// <summary>
    /// フラグを持っているか
    /// </summary>
    /// <param name="flagName"></param>
    /// <returns></returns>
    public bool HasFlag(string flagName) => flags.Contains(flagName);

    /// <summary>
    /// フラグをすべて呼び出す
    /// </summary>
    /// <typeparam name="string"></typeparam>
    /// <returns></returns>
    public List<string> GetAllFlags() => new List<string>(flags);

    /// <summary>
    /// 全消去（デバッグやリセット用）
    /// </summary>
    public void ClearAll()
    {
        flags.Clear();
    }

    public void LoadFromSaveData(List<string> loadedFlags)
    {
        flags = new HashSet<string>(loadedFlags);
    }
}
