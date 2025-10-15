using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T _prefab;
    private Transform _parent;
    private List<T> _pool = new List<T>();

    public ObjectPool(T prefab, int initialCount, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initialCount; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            _pool.Add(obj);
        }
    }

    public T GetObject()
    {
        foreach (T obj in _pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        // 空きがなければ新規作成
        T newObj = GameObject.Instantiate(_prefab, _parent);
        _pool.Add(newObj);
        return newObj;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }
}
