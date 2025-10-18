using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Event/EventDatabase")]
public class EventDatabase : ScriptableObject
{
    [SerializeField] private List<MultiStepEvent> events = new();
    private Dictionary<string, MultiStepEvent> dict;

    public static EventDatabase Instance;

    private void OnEnable()
    {
        Instance = this;
        dict = new Dictionary<string, MultiStepEvent>();
        foreach (var evt in events)
        {
            dict[evt.EventId] = evt;
        }
    }

    public MultiStepEvent GetEvent(string id)
    {
        dict ??= new Dictionary<string, MultiStepEvent>();
        if (dict.TryGetValue(id, out var evt))
            return evt;

        Debug.LogWarning($"Event not found: {id}");
        return null;
    }
}
