using System;

public interface IBattleEvent
{
    public int Time { get; }
    public EventType Type { get; }
    public String ActionName { get; }
    public IHeroUnit Hero { get; }
    public int EnemyId { get; }
    public IEnemyUnit Enemy { get; }
}