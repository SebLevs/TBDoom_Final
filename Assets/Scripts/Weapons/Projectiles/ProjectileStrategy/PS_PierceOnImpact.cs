using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/Piercing", fileName = "PiercingStrategySO")]
public class PS_PierceOnImpact : IProjectileStrategy
{
    [Header("Behaviour Values")]
    [SerializeField] private float damage;

    public override void ExecuteColliderStrategy()
    {
        //if (_projectile.LivingEntity != null && _projectile.LivingEntity.IsEnemy)
        //{
            m_projectile.ReadyForPooling = false;
        //}
    }

    public override void ExecuteRaycastStrategy()
    {
        //if (_projectile.LivingEntity != null && _projectile.LivingEntity.IsEnemy)
        //{
        //SpawnParticuleSystem(m_projectile.PointOfImpact);

        var currentDirection = (m_projectile.PointOfImpact - m_projectile.transform.position).normalized;
        var newPosition = m_projectile.PointOfImpact;
        var newRotation = m_projectile.transform.rotation;

        var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(newPosition + currentDirection.normalized * 0.01f, newRotation);
        newProjectile.Initialize(m_projectile.Weapon, m_projectile.Weapon.ProjectileDefinition, m_projectile.ExtractAllPS(), m_projectile.TargetMask, m_projectile.LivingEntity);
    //}
    }
}
