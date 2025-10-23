using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new();

    // 登録
    public static void Register<T>(T service)
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            UnityEngine.Debug.LogWarning($"{type.Name} はすでに登録されています。上書きします。");
        }
        _services[type] = service;
    }

    // 取得
    public static T Get<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
            return (T)service;

        throw new Exception($"{typeof(T).Name} は登録されていません。");
    }

    // 解除（不要なら省略可）
    public static void Unregister<T>()
    {
        _services.Remove(typeof(T));
    }
}
