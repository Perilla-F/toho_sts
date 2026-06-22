using System.Collections.Generic;

public interface IEnemyManager : IReadOnlyEnemyManager { }

public interface IReadOnlyEnemyManager
{
    public IEnemyUnit GetRandomAliveEnemy();
    public List<IEnemyUnit> GetAllEnemies();
}