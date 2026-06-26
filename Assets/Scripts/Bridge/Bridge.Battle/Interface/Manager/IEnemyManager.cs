using System.Collections.Generic;

public interface IEnemyManager : IReadOnlyEnemyManager
{
    /// <summary>
    /// idからエネミーを取り出す
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    new IEnemyUnit GetEnemy(int id);
    /// <summary>
    /// 生存エネミーからランダムに選択
    /// </summary>
    /// <returns></returns>
    new IEnemyUnit GetRandomAliveEnemy();
    /// <summary>
    /// 生存エネミーのリストを返す
    /// </summary>
    /// <returns></returns>
    new List<IEnemyUnit> GetAllEnemies();
    /// <summary>
    /// 生存エネミーのリストに登録
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enemy"></param>
    public void RegisterEnemy(int id, IEnemyUnit enemy);
    /// <summary>
    ///  生存エネミーのリストから除去
    /// </summary>
    /// <param name="id"></param>
    public void RemoveEnemy(int id);
    /// <summary>
    /// 生存エネミーをすべて除去
    /// </summary>
    public void ClearEnemies();
}

public interface IReadOnlyEnemyManager
{
    /// <summary>
    /// idからエネミーを取り出す
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IEnemyUnit GetEnemy(int id);
    /// <summary>
    /// 生存エネミーからランダムに選択
    /// </summary>
    /// <returns></returns>
    IReadOnlyEnemyUnit GetRandomAliveEnemy();
    /// <summary>
    /// 生存エネミーのリストを返す
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<IReadOnlyEnemyUnit> GetAllEnemies();
    /// <summary>
    /// 敵の全滅確認
    /// </summary>
    /// <returns></returns>
    bool AreAllEnemiesDefeated();
}