using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : Manager<T>
{
    private static T instance;

    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
