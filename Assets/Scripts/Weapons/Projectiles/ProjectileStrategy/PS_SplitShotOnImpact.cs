using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/SplitShot", fileName = "SplitShotProjectileStrategySO")]
public class PS_SplitShotOnImpact : IProjectileStrategy
{
    [Header("Behaviour Values")]
    [SerializeField] private float damage;
    [SerializeField] private int splits = 2;
    [SerializeField] private int splitHops = 2;

    public override void ExecuteColliderStrategy()
    {
        //if (_projectile.LivingEntity != null && _projectile.LivingEntity.IsEnemy)
        //{
        splitHops--;
        var splitAngleIncrement = 360 / splits;
        var currentSplitAngle = m_projectile.PhysicalProjectile.transform.forward;
        currentSplitAngle = Quaternion.Euler(0, splitAngleIncrement / splits, 0) * currentSplitAngle;
        for (int i = 0; i < splits; i++)
        {
            var newPosition = new Vector3(m_projectile.LivingEntity.transform.position.x, m_projectile.PointOfImpact.y, m_projectile.LivingEntity.transform.position.z);
            var newRotation = Quaternion.LookRotation(currentSplitAngle, Vector3.up);

            List<IProjectileStrategy> newStrategies = m_projectile.ExtractAllPS(this);
            if (splitHops > 0)
            {
                newStrategies.Add(GetCopy());
            }
            var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(newPosition, newRotation);
            newProjectile.Initialize(m_projectile.Weapon, m_projectile.Weapon.ProjectileDefinition, newStrategies, m_projectile.TargetMask, m_projectile.LivingEntity, this, m_projectile.Rigidbody.velocity);
            currentSplitAngle = Quaternion.Euler(0, splitAngleIncrement, 0) * currentSplitAngle;
        }
        //}
    }

    public override void ExecuteRaycastStrategy()
    {
        //if (_projectile.LivingEntity != null && _projectile.LivingEntity.IsEnemy)
        //{
        splitHops--;
        var splitAngleIncrement = 360 / splits;
        var currentSplitAngle = (m_raycastHit.point - m_projectile.transform.position).normalized;
        currentSplitAngle = Quaternion.Euler(0, splitAngleIncrement / splits, 0) * currentSplitAngle;
        for (int i = 0; i < splits; i++)
        {
            var newPosition = new Vector3(m_projectile.LivingEntity.transform.position.x, m_raycastHit.point.y, m_projectile.LivingEntity.transform.position.z);
            var newRotation = Quaternion.LookRotation(currentSplitAngle, Vector3.up);

            List<IProjectileStrategy> newStrategies = m_projectile.ExtractAllPS(this);
            if (splitHops > 0)
            {
                newStrategies.Add(GetCopy());
            }
            var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(newPosition, newRotation);
            newProjectile.Initialize(m_projectile.Weapon, m_projectile.Weapon.ProjectileDefinition, newStrategies, m_projectile.TargetMask, m_projectile.LivingEntity, this);
            currentSplitAngle = Quaternion.Euler(0, splitAngleIncrement, 0) * currentSplitAngle;
        }
        //}
    }

    private void ExecuteStrategy()
    {

    }
}
