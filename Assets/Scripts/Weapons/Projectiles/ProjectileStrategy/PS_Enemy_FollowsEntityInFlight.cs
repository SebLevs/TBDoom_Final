using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/Enemy/FollowsEntity", fileName = "FollowsEntityProjectileStrategySO")]
public class PS_Enemy_FollowsEntityInFlight : IProjectileStrategy
{
    private LivingEntityContext target = null;

    public override void ExecuteColliderStrategy()
    {
        ExecuteStrategy();
    }

    public override void ExecuteRaycastStrategy()
    {
        ExecuteStrategy();
    }

    private void ExecuteStrategy()
    {
        if (target == null)
        {
            target = m_projectile.PreviousLivingEntity;
            m_projectile.Rigidbody.velocity = Vector3.zero;
        }
        m_projectile.transform.position = target.transform.position;
        m_projectile.transform.rotation = target.transform.rotation;
    }
}
