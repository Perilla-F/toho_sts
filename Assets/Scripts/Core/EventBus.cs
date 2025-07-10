using System;
using System.Collections.Generic;

public static class EventBus<T>
{
    private static readonly List<Action<T>> _subscribers = new();

    public static void Publish(T message)
    {
        foreach (var sub in _subscribers)
            sub.Invoke(message);
    }

    public static void Subscribe(Action<T> handler)
    {
        _subscribers.Add(handler);
    }

    public static void Unsubscribe(Action<T> handler)
    {
        _subscribers.Remove(handler);
    }

    public static void Clear() => _subscribers.Clear();
}
