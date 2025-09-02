using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Event Database")]
public class EventDatabase : ScriptableObject
{
    public List<EventBase> EventList;

    private Dictionary<string, EventBase> _eventMap;

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        _eventMap = new Dictionary<string, EventBase>();
        foreach (var ev in EventList)
        {
            _eventMap[ev.EventId] = ev;
        }
    }

    public EventBase GetEventById(string id)
    {
        if (_eventMap == null || _eventMap.Count == 0) Init();
        if (_eventMap.TryGetValue(id, out var ev))
        {
            return ev;
        }

        Debug.LogWarning($"イベントID '{id}' が見つかりません");
        return null;
    }
}
