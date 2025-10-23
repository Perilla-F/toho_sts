public interface ISceneLoader
{
    /// <summary>
    /// 遷移前にデータをセット
    /// </summary>
    public void SetTransitionData(object data);

    /// <summary>
    /// 遷移先でデータ取得
    /// </summary>
    public T GetTransitionData<T>() where T : class;

    /// <summary>
    /// データ消去（必要なら）
    /// </summary>
    public void ClearTransitionData();

    /// <summary>
    /// シーンロード
    /// </summary>
    public void LoadScene(string sceneName);

}