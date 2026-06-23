using System.Collections.Generic;

public interface ITimelineManager : IReadOnlyTimelineManager
{
    public void AddEvent(BattleEvent e);
}

public interface IReadOnlyTimelineManager
{
    public int CurrentTime { get; }
    public List<BattleEvent> GetUpcomingEvents();
}