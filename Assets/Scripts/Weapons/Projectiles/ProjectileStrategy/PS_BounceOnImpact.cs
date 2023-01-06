using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/Bounce", fileName = "BounceStrategySO")]
public class PS_BounceOnImpact : IProjectileStrategy
{
    [Header("Behaviour Values")]
    [SerializeField] private float damage;
    [SerializeField] private int bounceHops = 1;

    public override void ExecuteColliderStrategy()
    {
        //bool check = true;
        //if (m_projectile.LivingEntity != null)
        //{
        //    foreach (IProjectileStrategy strategy in m_projectile.ProjectileStrategies)
        //    {
        //        Debug.Log(strategy.name);
        //        if (strategy is PS_PierceOnImpact)
        //        {
        //            Debug.Log("Can't Bounce");
        //            check = false;
        //            break;
        //        }
        //    }
        //}

        //if (check)
        //{
            //SpawnParticuleSystem(m_projectile.PointOfImpact);

            bounceHops--;
            var currentDirection = m_projectile.Rigidbody.velocity.normalized;
            var reflectDirection = Vector3.Reflect(currentDirection, m_projectile.NormalOfImpact.normalized);
            var newPosition = m_projectile.PointOfImpact + m_projectile.ImpactOffset + reflectDirection * m_projectile.ProjectileOffset; // _projectile.PointOfImpact + reflectDirection * 0.5f;
            var newRotation = Quaternion.LookRotation(reflectDirection, Vector3.up);

            List<IProjectileStrategy> newStrategies = m_projectile.ExtractAllPS(this);
            if (bounceHops > 0)
            {
                newStrategies.Add(GetCopy());
            }
            var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(newPosition, newRotation);
            newProjectile.Initialize(m_projectile.Weapon, m_projectile.Weapon.ProjectileDefinition, newStrategies, m_projectile.TargetMask, null, null, m_projectile.Rigidbody.velocity);
        //}
    }

    public override void ExecuteRaycastStrategy()
    {
        //bool check = true;
        //if (m_projectile.LivingEntity != null)
        //{
        //    foreach (IProjectileStrategy strategy in m_projectile.ProjectileStrategies)
        //    {
        //        if (strategy is PS_PierceOnImpact)
        //        {
        //            Debug.Log("Can't Bounce");
        //            check = false;
        //            break;
        //        }
        //    }
        //}

        //if (check)
        //{
            //SpawnParticuleSystem(m_raycastHit.point);

            bounceHops--;
            var currentDirection = (m_raycastHit.point - m_projectile.transform.position).normalized;
            var reflectDirection = Vector3.Reflect(currentDirection, m_raycastHit.normal);
            var newPosition = new Vector3(m_raycastHit.point.x, m_raycastHit.point.y, m_raycastHit.point.z);
            var newRotation = Quaternion.LookRotation(reflectDirection, Vector3.up);

            List<IProjectileStrategy> newStrategies = m_projectile.ExtractAllPS(this);
            if (bounceHops > 0)
            {
                newStrategies.Add(GetCopy());
            }
            var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(newPosition, newRotation);
            newProjectile.Initialize(m_projectile.Weapon, m_projectile.Weapon.ProjectileDefinition, newStrategies, m_projectile.TargetMask, null);
        //}
    }
}
