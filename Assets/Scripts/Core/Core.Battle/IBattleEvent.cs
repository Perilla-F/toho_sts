public interface IBattleEvent
{
    public int Time { get; }
    public EventType Type { get; }
    public int EnemyId { get; }
    public IEnemyUnit Enemy { get; }
}