using System.Collections.Generic;

public interface ITimelineManager
{
    public int GetCurrentTime();
    public IReadOnlyList<IBattleEvent> GetUpcomingEvents();
}