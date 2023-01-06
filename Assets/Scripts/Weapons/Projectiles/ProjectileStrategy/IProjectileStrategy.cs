using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileStrategyExecutionType { SPAWN, FLIGHT, IMPACT }

public abstract class IProjectileStrategy : ScriptableObject
{
    [SerializeField] protected bool isDebugOn = false;
    [SerializeField] protected ProjectileStrategyType m_projectileStrategyType;
    [SerializeField] protected ProjectileStrategyExecutionType m_projectileStrategyExecutionType;
    [SerializeField] protected ProjectileStrategyType[] m_exceptionProjectileStrategyType;
    // [SerializeField] protected ProjectileSpawner projectileSpawner;
    [SerializeField] protected string m_strategyName;
    [SerializeField] protected string m_strategyDescription;
    [SerializeField] protected float m_damageMultiplier;
    [SerializeField] protected LayerMask m_targetMask;
    [SerializeField] protected float m_probability = 100;
    [SerializeField] protected bool m_onlyAppliesOnLivingEntity;
    [SerializeField] protected ParticleSystem m_particleSystem;
    [SerializeField] protected Sprite m_sprite;
    
    protected LivingEntityContext m_livingEntity;
    protected Projectile m_projectile;
    protected Collider m_collider;
    protected RaycastHit m_raycastHit;
    protected IProjectileStrategy m_initialStrategy;
    protected Transform m_projectileOrigin;
    protected float m_damage;

    public ProjectileStrategyExecutionType ProjectileStrategyExecutionType { get => m_projectileStrategyExecutionType; set => m_projectileStrategyExecutionType = value; }
    public ProjectileStrategyType ProjectileStrategyType { get => m_projectileStrategyType; set => m_projectileStrategyType = value; }
    // public ProjectileSpawner ProjectileSpawner { get => projectileSpawner; set => projectileSpawner = value; }
    public string StrategyName { get => m_strategyName; set => m_strategyName = value; }
    public string StrategyDescription { get => m_strategyDescription; set => m_strategyDescription = value; }
    public LayerMask TargetMask { get => m_targetMask; set => m_targetMask = value; }
    public IProjectileStrategy InitialStrategy { get => m_initialStrategy; set => m_initialStrategy = value; }
    public Sprite Sprite { get => m_sprite; set => m_sprite = value; }
    public float DamageMultiplier { get => m_damageMultiplier; set => m_damageMultiplier = value; }

    public void BaseExecuteStrategy(Collider _collider, Projectile _projectile)
    {
        if (!InitializeStrategy(_projectile))
            return;

        m_collider = _collider;
        if (m_collider != null)
        {
            m_livingEntity = m_collider.GetComponent<LivingEntityContext>();
        }
        ExecuteColliderStrategy();
    }

    public abstract void ExecuteColliderStrategy();

    public void BaseExecuteStrategy(RaycastHit _raycastHit, Projectile _projectile)
    {
        if (!InitializeStrategy(_projectile))
            return;

        m_raycastHit = _raycastHit;
        ExecuteRaycastStrategy();
    }

    public abstract void ExecuteRaycastStrategy();

    private bool InitializeStrategy(Projectile _projectile)
    {
        m_projectile = _projectile;
        m_livingEntity = m_projectile.LivingEntity;

        if (m_onlyAppliesOnLivingEntity && m_livingEntity == null)
            return false;

        if (RandomManager.Instance.OtherRandom.Random.Next(0, 100) >= m_probability)
            return false;

        if (m_livingEntity != null)
        {
            foreach (ProjectileStrategyType strategyType in m_exceptionProjectileStrategyType)
            {
                if (m_projectile.ProjectileStrategies.Exists(i => i.ProjectileStrategyType == strategyType))
                {
                    return false;
                }
            }
        }

        if (m_particleSystem != null)
        {
            SpawnParticuleSystem(m_projectile.PointOfImpact);
        }

        if (m_targetMask == 0)
        {
            m_targetMask = _projectile.TargetMask;
        }
        
        m_projectileOrigin = _projectile.transform;

        return true;
    }

    public void SpawnParticuleSystem(Vector3 _point)
    {
        if (m_particleSystem == null) return;

        var newParticleSystem = Instantiate(m_particleSystem);
        newParticleSystem.transform.localPosition = _point;
        newParticleSystem.transform.localRotation = Quaternion.identity;
        newParticleSystem.transform.localScale = Vector3.one;
        newParticleSystem.transform.SetParent(null);
    }

    public IProjectileStrategy GetCopy()
    {
        var copy = Instantiate(this);
        return copy;
    }
}
