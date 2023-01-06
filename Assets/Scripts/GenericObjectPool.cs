using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericObjectPool<T> : MonoBehaviour, IObjectPool<T> where T : MonoBehaviour, IPoolableObject
{
    [SerializeField] private SO_ObjectPoolDefinition m_objectPoolDefinition;
    [SerializeField] private T m_prefabInstance;
    private Queue<T> m_availableInstances = new Queue<T>();

    public void Initialize()
    {
        for (int i = 0; i < m_objectPoolDefinition.MaxRecycledEntities; i++)
        {
            ReturnToPool(Instantiate(m_prefabInstance));
        }
    }

    public virtual T GetPrefabInstance(Vector3 _position, Quaternion _rotation)
    {
        T instance;

        if (m_availableInstances.Count > 0)
        {
            instance = m_availableInstances.Dequeue();
        }
        else
        {
            instance = Instantiate(m_prefabInstance);
            m_objectPoolDefinition.MaxRecycledEntities++;
        }

        instance.Origin = this;
        instance.PrepareToUse(_position, _rotation);
        
        return instance;
    }

    public void ReturnToPool(T _instance)
    {
        _instance.gameObject.SetActive(false);
        _instance.transform.SetParent(transform);

        _instance.transform.localScale = Vector3.one;
        _instance.transform.localPosition = Vector3.zero;
        _instance.transform.localRotation = Quaternion.identity;

        m_availableInstances.Enqueue(_instance);
    }

    public void ReturnToPool(object _instance)
    {
        if (_instance is T)
            ReturnToPool(_instance as T);
    }
}
