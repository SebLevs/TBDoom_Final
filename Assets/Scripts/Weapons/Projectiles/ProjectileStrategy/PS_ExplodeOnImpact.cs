using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/ExplodeOnImpact", fileName = "ExplodeOnImpactProjectileStrategySO")]
public class PS_ExplodeOnImpact : IProjectileStrategy
{
    // SECTION - Field ============================================================
    [Header("SphereCast Values")]
    [SerializeField] private float m_explosionRadius;

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
        m_damage = m_projectile.Damage * m_damageMultiplier;
        var collArray = StaticRayCaster.IsOverlapSphereTouching(m_projectile.PointOfImpact, m_explosionRadius, m_targetMask, isDebugOn);
        foreach (Collider hitObj in collArray)
        {
            if (hitObj.GetComponent<LivingEntityContext>())
            {
                hitObj.GetComponent<LivingEntityContext>().TakeDamage(m_damage, m_projectileOrigin.position);
            }
            if (hitObj.GetComponentInParent<Block>())
            {
                if (hitObj.GetComponentInParent<Block>().IsBreakable)
                {
                    Destroy(hitObj.gameObject);
                }
            }
        }
    }
}
