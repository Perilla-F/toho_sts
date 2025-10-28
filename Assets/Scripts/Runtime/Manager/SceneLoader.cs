using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISceneLoader
{
    private object _transitionData;

    private void Awake()
    {
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

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            yield return null;
        }
    }
    #endregion
}
