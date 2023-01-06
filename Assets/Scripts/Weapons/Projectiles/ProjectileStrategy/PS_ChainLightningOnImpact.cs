using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/ChainLightning", fileName = "ChainLightningProjectileStrategySO")]
public class PS_ChainLightningOnImpact : IProjectileStrategy
{
    [Header("SphereCast Values")]
    [SerializeField] private float radius;

    [Header("Behaviour Values")]
    [SerializeField] private float damage;
    [SerializeField] private int lightningHops;
    [SerializeField] private SO_ProjectileDefinition m_projectileDefinition;
    private List<LivingEntityContext> m_touchedEnemis = new List<LivingEntityContext>();
    private PS_ChainLightningOnImpact m_mainChainLightning;

    public List<LivingEntityContext> TouchedEnemis { get => m_touchedEnemis; set => m_touchedEnemis = value; }
    public PS_ChainLightningOnImpact MainChainLightning { get => m_mainChainLightning; set => m_mainChainLightning = value; }

    public override void ExecuteColliderStrategy()
    {
        ExecuteStrategy();
        //if (_projectile.LivingEntity != null && _projectile.LivingEntity.IsEnemy)
        //{
        //    if (!m_mainChainLightning.m_touchedEnemis.Contains(_projectile.LivingEntity))
        //    {
        //        m_mainChainLightning.m_touchedEnemis.Add(_projectile.LivingEntity);
        //    }

        //    SpawnParticuleSystem(_projectile.PointOfImpact);

        //    lightningHops--;
        //    var collArray = StaticRayCaster.IsOverlapSphereTouching(_projectile.PointOfImpact, radius, m_targetMask, isDebugOn);
        //    foreach (Collider hitObj in collArray)
        //    {
        //        var radiusLivingEntity = hitObj.GetComponent<LivingEntityContext>();
        //        if (radiusLivingEntity != null && radiusLivingEntity.IsEnemy && !m_mainChainLightning.m_touchedEnemis.Contains(radiusLivingEntity))
        //        {
        //            m_mainChainLightning.m_touchedEnemis.Add(radiusLivingEntity);
        //            var hitObjPosition = new Vector3(hitObj.transform.position.x, _projectile.PointOfImpact.y, hitObj.transform.position.z);
        //            var newRotation = Quaternion.LookRotation(hitObjPosition - _projectile.PointOfImpact, Vector3.up);

        //            List<IProjectileStrategy> newStrategies = new List<IProjectileStrategy>();
        //            if (lightningHops > 0)
        //            {
        //                var newLigthningStrategy = GetCopy();
        //                (newLigthningStrategy as PS_ChainLightningOnImpact).MainChainLightning = m_mainChainLightning;
        //                newStrategies.Add(newLigthningStrategy);
        //            }
        //            var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(_projectile.LivingEntity.transform.position, newRotation);
        //            newProjectile.Initialize(_projectile.Weapon, m_projectileDefinition, newStrategies, _projectile.TargetMask, null);
        //        }
        //    }
        //}
    }

    public override void ExecuteRaycastStrategy()
    {
        ExecuteStrategy();
        //if (_projectile.LivingEntity != null && _projectile.LivingEntity.IsEnemy)
        //{
        //    if (!m_mainChainLightning.m_touchedEnemis.Contains(_projectile.LivingEntity))
        //    {
        //        m_mainChainLightning.m_touchedEnemis.Add(_projectile.LivingEntity);
        //    }

        //    SpawnParticuleSystem(_raycastHit.point);

        //    lightningHops--;
        //    var collArray = StaticRayCaster.IsOverlapSphereTouching(_raycastHit.point, radius, m_targetMask, isDebugOn);
        //    foreach (Collider hitObj in collArray)
        //    {
        //        var radiusLivingEntity = hitObj.GetComponent<LivingEntityContext>();
        //        if (radiusLivingEntity != null && radiusLivingEntity.IsEnemy && !m_mainChainLightning.m_touchedEnemis.Contains(radiusLivingEntity))
        //        {
        //            m_mainChainLightning.m_touchedEnemis.Add(radiusLivingEntity);
        //            var hitObjPosition = new Vector3(hitObj.transform.position.x, _raycastHit.point.y, hitObj.transform.position.z);
        //            var newRotation = Quaternion.LookRotation(hitObjPosition - _raycastHit.point, Vector3.up);

        //            List<IProjectileStrategy> newStrategies = new List<IProjectileStrategy>();
        //            if (lightningHops > 0)
        //            {
        //                var newLigthningStrategy = GetCopy();
        //                (newLigthningStrategy as PS_ChainLightningOnImpact).MainChainLightning = m_mainChainLightning;
        //                newStrategies.Add(newLigthningStrategy);
        //            }
        //            var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(_projectile.LivingEntity.transform.position, newRotation);
        //            newProjectile.Initialize(_projectile.Weapon, m_projectileDefinition, newStrategies, _projectile.TargetMask, null);
        //        }
        //    }
        //}
    }

    private void ExecuteStrategy()
    {
        if (!m_mainChainLightning.m_touchedEnemis.Contains(m_projectile.LivingEntity))
        {
            m_mainChainLightning.m_touchedEnemis.Add(m_projectile.LivingEntity);
        }

        //SpawnParticuleSystem(m_projectile.PointOfImpact);

        lightningHops--;
        var collArray = StaticRayCaster.IsOverlapSphereTouching(m_livingEntity.transform.position, radius, m_targetMask, isDebugOn);
        foreach (Collider hitObj in collArray)
        {
            var radiusLivingEntity = hitObj.GetComponent<LivingEntityContext>();
            if (radiusLivingEntity != null && radiusLivingEntity.IsEnemy && !m_mainChainLightning.m_touchedEnemis.Contains(radiusLivingEntity))
            {
                m_mainChainLightning.m_touchedEnemis.Add(radiusLivingEntity);
                var hitObjPosition = new Vector3(hitObj.transform.position.x, m_projectile.PointOfImpact.y, hitObj.transform.position.z);
                var newDirection = new Vector3(hitObjPosition.x - m_livingEntity.transform.position.x, 0, hitObjPosition.z - m_livingEntity.transform.position.z);
                var newRotation = Quaternion.LookRotation(newDirection, Vector3.up);

                List<IProjectileStrategy> newStrategies = new List<IProjectileStrategy>();
                if (lightningHops > 0)
                {
                    var newLigthningStrategy = GetCopy();
                    (newLigthningStrategy as PS_ChainLightningOnImpact).MainChainLightning = m_mainChainLightning;
                    newStrategies.Add(newLigthningStrategy);
                }
                var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(m_livingEntity.transform.position, newRotation);
                newProjectile.Initialize(m_projectile.Weapon, m_projectileDefinition, newStrategies, m_projectile.TargetMask, null, this);
            }
        }
    }
}