using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Event Database")]
public class EventDatabase : ScriptableObject
{
    public List<EventBase> eventList;

    private Dictionary<string, EventBase> _eventMap;

    public void Init()
    {
        _eventMap = new Dictionary<string, EventBase>();
        foreach (var ev in eventList)
        {
            _eventMap[ev.eventId] = ev;
        }
    }

    public EventBase GetEventById(string id)
    {
        if (_eventMap == null) Init();
        if (_eventMap.TryGetValue(id, out var ev))
        {
            return ev;
        }

        Debug.LogWarning($"イベントID '{id}' が見つかりません");
        return null;
    }
}
