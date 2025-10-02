using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移とデータ保持用シングルトン
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private object _transitionData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region 遷移データ管理
    /// <summary>
    /// 遷移前にデータをセット
    /// </summary>
    public void SetTransitionData(object data)
    {
        _transitionData = data;
    }

    /// <summary>
    /// 遷移先でデータ取得
    /// </summary>
    public T GetTransitionData<T>() where T : class
    {
        if (_transitionData is T t) return t;
        return null;
    }

    /// <summary>
    /// データ消去（必要なら）
    /// </summary>
    public void ClearTransitionData()
    {
        _transitionData = null;
    }
    #endregion

    #region シーン遷移
    /// <summary>
    /// シーンロード
    /// </summary>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            yield return null;
        }
    }
    #endregion
}
