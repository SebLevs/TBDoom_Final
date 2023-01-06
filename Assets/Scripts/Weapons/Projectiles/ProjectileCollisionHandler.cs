using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionHandler : MonoBehaviour
{
    [SerializeField] private Projectile m_projectile;

    //private void OnCollisionEnter(Collider _collision)
    //{
    //    m_projectile.Impact(_collision);
    //}

    private void OnTriggerEnter(Collider _other)
    {
        m_projectile.Impact(_other);
    }
}
