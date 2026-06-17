using System.Collections.Generic;

public interface ITimelineManager
{
    public List<BattleEvent> GetUpcomingEvents();
}