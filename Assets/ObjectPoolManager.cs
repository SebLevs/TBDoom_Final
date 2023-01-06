using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    [SerializeField] private GenericObjectPool<Projectile> m_projectilePool;

    public GenericObjectPool<Projectile> ProjectilePool { get => m_projectilePool; set => m_projectilePool = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        m_projectilePool.Initialize();
    }
}
