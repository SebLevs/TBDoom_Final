using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/AutoAim", fileName = "AutoAimProjectileStrategySO")]
public class PS_AutoAimInFlight : IProjectileStrategy
{
    [Header("SphereCast Values")]
    [SerializeField] private float radius;
    [SerializeField] private float capsuleRadius = 1f;

    [Header("Behaviour Values")]
    [SerializeField] private float damage;
    [SerializeField] private Projectile projectile;
    [SerializeField] private float lerpIntensity = 0.1f;
    private LivingEntityContext target = null;

    public override void ExecuteColliderStrategy()
    {
        var collArray = StaticRayCaster.IsOverlapSphereTouching(m_projectile.PhysicalProjectile.transform.position, radius, m_targetMask, isDebugOn); // transform.parent.transform
        foreach (Collider hitObj in collArray)
        {
            var radiusLivingEntity = hitObj.GetComponent<LivingEntityContext>();
            if (radiusLivingEntity != null && radiusLivingEntity.IsEnemy)
            {
                if (target == null)
                {
                    target = radiusLivingEntity;
                    Debug.Log("Target is " + target);
                }
                else
                {
                    var distance = (m_projectile.PhysicalProjectile.transform.position - hitObj.transform.position).magnitude;
                    if (distance < (m_projectile.PhysicalProjectile.transform.position - target.transform.position).magnitude)
                    {
                        target = radiusLivingEntity;
                        Debug.Log("Target is " + target);
                    }
                }
            }
        }

        if (target != null)
        {
            var targetForward = target.transform.position - m_projectile.PhysicalProjectile.transform.position;
            var currentForward = m_projectile.PhysicalProjectile.transform.forward;
            m_projectile.UpdateVelocity(targetForward);//Vector3.Lerp(currentForward, targetForward, lerpIntensity));
            // Debug.Log("New forward is " + _projectile.PhysicalProjectile.transform.forward);
        }
    }

    public override void ExecuteRaycastStrategy()
    {
        var capsuleCollisions = Physics.OverlapCapsule(m_projectile.transform.position, m_raycastHit.point, capsuleRadius, m_targetMask);
        // var collArray = StaticRayCaster.IsOverlapSphereTouching(currentProjectile.transform.position, radius, myTargetMask, isDebugOn); // transform.parent.transform
        foreach (Collider hitObj in capsuleCollisions)
        {
            var radiusLivingEntity = hitObj.GetComponent<LivingEntityContext>();
            if (radiusLivingEntity != null && radiusLivingEntity.IsEnemy)
            {
                if (target == null)
                {
                    target = radiusLivingEntity;
                    Debug.Log("Target is " + target);
                }
                else
                {
                    var distance = (m_raycastHit.point - hitObj.transform.position).magnitude;
                    if (distance < (m_raycastHit.point - target.transform.position).magnitude)
                    {
                        target = radiusLivingEntity;
                        Debug.Log("Target is " + target);
                    }
                }
            }
        }

        if (target != null)
        {
            var newDirection = target.transform.position - m_projectile.transform.position;
            newDirection = new Vector3(newDirection.x, 0, newDirection.z);
            RaycastHit newRaycastHit;
            newRaycastHit = StaticRayCaster.IsLineCastTouching(m_projectile.transform.position, newDirection, 1000, m_targetMask, isDebugOn);
            if (newRaycastHit.collider != null)
            {
                m_projectile.RaycastHit = newRaycastHit;
            } 
        }
    }
}
