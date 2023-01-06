using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    public void ReturnToPool(object _instance);
}

public interface IObjectPool<T> : IObjectPool where T : IPoolableObject
{
    public void Initialize();
    public T GetPrefabInstance(Vector3 _position, Quaternion _rotation);
    public void ReturnToPool(T _instance);
}
