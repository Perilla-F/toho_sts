public interface IEnemyActionEvent
{
    public IEnemyUnit Enemy { get; }
    public IEnemyManager ReferenceEnemy { get; }
}